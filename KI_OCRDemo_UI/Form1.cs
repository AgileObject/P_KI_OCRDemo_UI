using KI_OCRDemo_UI.OCR;
using KI_OCRDemo_UI.Azure_Secrets;
using KI_OCRDemo_UI.AWS_Textract_Secrets;
using log4net.Config;
using log4net;
using System.Reflection;
using System.Configuration;
using Amazon.Textract.Model;
using Amazon.Textract;
using KI_OCRDemo_UI.OpenAI;
using KI_OCRDemo_UI.OPENAI_Secrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace KI_OCRDemo_UI
{
    public partial class mainFormOCRDemo : Form
    {
        #region Declaration
        private static readonly ILog _Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _SelectedFileName = string.Empty;
        private string lastDirectory = @"C:\"; // Standardverzeichnis
        #endregion

        #region Constructor
        public mainFormOCRDemo()
        {
            InitializeComponent();
            InitializeAppl();

        }
        #endregion

        #region ApplicationHandling
        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = lastDirectory;
                openFileDialog.Filter = "image (*.jpg)|*.jpg|image (*.jpeg)|*.jpeg|image (*.png)|*.png|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hier wird die ausgewählte Datei verarbeitet
                    _SelectedFileName = openFileDialog.FileName;
                    labelSelectedFile.Text = _SelectedFileName;
                    pictureBoxSelectedFile.Image = Image.FromFile(_SelectedFileName);
                    pictureBoxSelectedFile.SizeMode = PictureBoxSizeMode.Zoom;
                    _Logger.Info(_SelectedFileName);
                    lastDirectory = System.IO.Path.GetDirectoryName(_SelectedFileName);
                }
            }
        }

        private async void buttonDoOCR_Click(object sender, EventArgs e)
        {
            if (checkBoxKIVision.Checked)
            {
                _Logger.Info("Starte OCR Azure Computer Vision...");
                string resultFileName = azureComputerVision.ImageAnalysisSample_Analyze_File(_SelectedFileName);
                string text = File.ReadAllText(resultFileName);
                textBoxResultOCR.Text = text.Replace("\n", Environment.NewLine);
            }
            else if (checkBoxTextract.Checked)
            {
                _Logger.Info("Starte OCR AWS Textract...");
                string resultFileName = await textractOCR.GetOCRResultFromFile(_SelectedFileName);
                string text = File.ReadAllText(resultFileName);
                textBoxResultOCR.Text = text.Replace("\n", Environment.NewLine);
            }
            _Logger.Info("OCR finished!");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxKIVision_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKIVision.Checked)
            {
                checkBoxTextract.Checked = false;
            }
            else
            {
                checkBoxKIVision.Enabled = true;
            }
        }

        private void checkBoxTextract_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTextract.Checked)
            {
                checkBoxKIVision.Checked = false;
            }
            else
            {
                checkBoxTextract.Enabled = true;
            }
        }

        private async void buttonOptimizeGPT_Click(object sender, EventArgs e)
        {
            //var chatGPTService = new ChatGptService();
            ChatGptService chatGptService = new ChatGptService();

            string correctedText = await chatGptService.CorrectTextAsync(textBoxResultOCR.Text.ToString());

            textBoxGPTResult.Text = correctedText.Replace("\n", Environment.NewLine);
        }

        private async void buttonDirectReadingGPT_Click(object sender, EventArgs e)
        {
            ChatGptService chatGptService = new ChatGptService();

            string promptText = textBoxGPTPrompt.Text.ToString();
            if(String.IsNullOrEmpty(promptText))
            {
                promptText = "Welcher Text ist im Bild enthalten?\n";
            }

            //return ist ein JSON 
            string correctedText = await chatGptService.ImageAnalysis(_SelectedFileName, promptText);

            try
            {
                // Parse des JSON-Strings in ein JObject
                var jsonObject = JObject.Parse(correctedText);

                // Zugriff auf den "content"-String im JSON
                string content = jsonObject["choices"]?[0]?["message"]?["content"]?.ToString();

                // Überprüfen, ob content tatsächlich existiert, bevor es zugewiesen wird
                if (!string.IsNullOrEmpty(content))
                {
                    // Anzeigen des Inhalts in der TextBox
                    textBoxGPTResult.Text = content.Replace("\n", Environment.NewLine); 
                }
                else
                {
                    MessageBox.Show("Content nicht gefunden");
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                MessageBox.Show($"Fehler beim Verarbeiten des JSON: {ex.Message}");
            }

            //textBoxGPTResult.Text = correctedText.Replace("\n", Environment.NewLine);
        }


        #endregion

        #region Initialize
        private void InitializeAppl()
        {
            _Logger.Info("Initializing application " + this.Text + " " +
                ConfigurationManager.AppSettings["ApplicationVersion"].ToString());
            InitGPTPrompt();
            InitCheckButtons();
            InitPictureBox();
            InitAzureVisionSecrets();
            InitAWSTextractSecrets();
            InitOpenAIAPI();
            _Logger.Info("...finished");
        }

        private void InitGPTPrompt()
        {
            textBoxGPTPrompt.Text = "Welcher Text ist im Bild enthalten?\n";
        }

        private void InitPictureBox()
        {
            string path2InitialPicture = ConfigurationManager.AppSettings["InitPictureBox"].ToString();
            pictureBoxSelectedFile.Image = Image.FromFile(path2InitialPicture);
        }

        private void InitCheckButtons()
        {
            checkBoxKIVision.Checked = true;
            checkBoxTextract.Checked = false;
        }

        private void InitAzureVisionSecrets()
        {
            _Logger.Info("Reading Azure KI Vision secret's");
            if (!AzureSecrets.LoadSucceeded(null))
            {
                string errorText = "Azure KI Vision Key/Endpoint not found!\r\nClosing application";
                MessageBox.Show(errorText, "Error", MessageBoxButtons.OK);
                this.Close();
            }
        }

        private void InitAWSTextractSecrets()
        {
            _Logger.Info("Reading AWS Textract secret's");
            if (!AWSSecrets.LoadSucceeded(null))
            {
                string errorText = "AWS_Access_KeyID/AWS_Secret_Access_key not found!\r\nClosing application";
                MessageBox.Show(errorText, "Error", MessageBoxButtons.OK);
                this.Close();
            }
        }
        private void InitOpenAIAPI()
        {
            _Logger.Info("Reading OPENAI secret's");
            if (!OpenAISecrets.LoadSucceeded(null))
            {
                string errorText = "OPENAI APIKey/BaseAddress not found!\r\nClosing application";
                MessageBox.Show(errorText, "Error", MessageBoxButtons.OK);
                this.Close();
            }
        }

        #endregion

    }
}
