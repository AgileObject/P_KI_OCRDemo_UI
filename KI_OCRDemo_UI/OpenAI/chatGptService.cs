using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.Runtime.Internal;
using Azure;
using KI_OCRDemo_UI.OPENAI_Secrets;
using log4net;

namespace KI_OCRDemo_UI.OpenAI
{
    public class ChatGptService
    {
        private static readonly ILog _Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static HttpClient httpClient;

        public void ChatGPTService()
        {
        }

        public async Task<string> CorrectTextAsync(string inputText)
        {
            _Logger.Info("Start text processing...");
            httpClient = new HttpClient
            {
                //BaseAddress = new Uri("https://api.openai.com/v1/")
                BaseAddress = new Uri(OpenAISecrets.BaseAddress)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAISecrets.APIKey);

            var requestBody = new
            {
                model = "gpt-4o", // Verwende gpt-4, wenn du Zugriff hast
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant that corrects grammar and spelling." },
                    new { role = "user", content = $"Please correct the following text: {inputText}" }
                }
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("chat/completions", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            string correctedText = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            _Logger.Info("Start text processing... finished!");

            return correctedText.Trim();
        }

        public async Task<string> ImageAnalysis(string imagePath, string promptText)
        {
            string answer = string.Empty;

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);

            var requestData = new
            {
                model = "gpt-4o",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = new object[]
                        {
                            new { type = "text", text = "Du bist ein hilfreicher Assistent der Text aus Bildern liest" }
                        }
                    },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = promptText },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                        }
                    }
                },
            };

            using (var client1 = new HttpClient())
            {
                client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAISecrets.APIKey);
                client1.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response1 = client1.PostAsync("https://api.openai.com/v1/chat/completions", httpContent).Result;

                if (response1.IsSuccessStatusCode)
                {
                    Task<string> taskResult = response1.Content.ReadAsStringAsync();
                    if (String.IsNullOrEmpty(taskResult.Result))
                    {
                        answer = String.Empty;
                    }
                    else
                    {
                        answer = taskResult.Result;
                    }
                }
                else
                {
                    Task<string> errorResponse = response1.Content.ReadAsStringAsync();
                    //Console.WriteLine("HTTP Error: {response1.StatusCode}");
                    answer = response1.StatusCode + " - " + response1.ReasonPhrase + " - " + errorResponse.Result;
                }
            }
            return answer;
        }

    }

}


