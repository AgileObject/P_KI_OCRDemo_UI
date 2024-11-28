using Azure;
using Azure.AI.Vision.Common;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using log4net;
using System.Reflection;
using KI_OCRDemo_UI.Azure_Secrets;

namespace KI_OCRDemo_UI.OCR
{
    public class azureComputerVision
    {
        private static readonly ILog _Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string ImageAnalysisSample_Analyze_File(string fileName)
        {
            DateTime dateTime = DateTimeOffset.Now.DateTime;
            string timeOCRStr = dateTime.Year.ToString();
            timeOCRStr += dateTime.Month.ToString();
            timeOCRStr += dateTime.Day.ToString();
            timeOCRStr += dateTime.Hour.ToString();
            timeOCRStr += dateTime.Minute.ToString();
            timeOCRStr += dateTime.Second.ToString();

            _Logger.Info("Generate OCR result file: " + timeOCRStr + "_AICV_OCR_Result.txt");

            StreamWriter writer = new StreamWriter(timeOCRStr + "_AICV_OCR_Result.txt");
            var serviceOptions = new VisionServiceOptions(AzureSecrets.Endpoint, new AzureKeyCredential(AzureSecrets.Key));

            // Specify the image file on disk to analyze. sample.jpg is a good example to show most features.
            // Alternatively, specify an image URL (e.g. https://aka.ms/azai/vision/image-analysis-sample.jpg)
            // or a memory buffer containing the image. see:
            // https://learn.microsoft.com/azure/ai-services/computer-vision/how-to/call-analyze-image-40?pivots=programming-language-csharp#select-the-image-to-analyze
            //Console.WriteLine("Filename: ");
            //string fileName = Console.ReadLine();
            //using var imageSource = VisionSource.FromFile("sample.jpg");
            using var imageSource = VisionSource.FromFile(fileName);

            var analysisOptions = new ImageAnalysisOptions()
            {
                // Mandatory. You must set one or more features to analyze. Here we use the full set of features.
                // Note that 'Caption' and 'DenseCaptions' are only supported in Azure GPU regions (East US, France Central, Korea Central,
                // North Europe, Southeast Asia, West Europe, West US). Remove 'Caption' and 'DenseCaptions' from the list below if your
                // Computer Vision key is not from one of those regions.
                Features =
                    //  ImageAnalysisFeature.CropSuggestions
                    //  ImageAnalysisFeature.Caption not supported region
                    //  ImageAnalysisFeature.DenseCaptions not supported region
                    ImageAnalysisFeature.Objects
                    | ImageAnalysisFeature.People
                    | ImageAnalysisFeature.Text
                    | ImageAnalysisFeature.Tags,

                // Optional, and only relevant when you select ImageAnalysisFeature.CropSuggestions.
                // Define one or more aspect ratios for the desired cropping. Each aspect ratio needs to be in the range [0.75, 1.8].
                // If you do not set this, the service will return one crop suggestion with the aspect ratio it sees fit.
                //CroppingAspectRatios = new List<double>() { 0.75, 1.8 },

                // Optional. Default is "en" for English. See https://aka.ms/cv-languages for a list of supported
                // language codes and which visual features are supported for each language.
                Language = "en",

                // Optional. Default is "latest".
                ModelVersion = "latest",

                // Optional, and only relevant when you select ImageAnalysisFeature.Caption.
                // Set this to "true" to get a gender neutral caption (the default is "false").
                GenderNeutralCaption = true
            };

            using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

            _Logger.Debug(" Please wait for image analysis results...\n");

            // This call creates the network connection and blocks until Image Analysis results
            // return (or an error occurred). Note that there is also an asynchronous (non-blocking)
            // version of this method: analyzer.AnalyzeAsync().
            var result = analyzer.Analyze();

            if (result.Reason == ImageAnalysisResultReason.Analyzed)
            {
                writer.WriteLine($" Image height = {result.ImageHeight}");
                writer.WriteLine($" Image width = {result.ImageWidth}");
                writer.WriteLine($" Model version = {result.ModelVersion}");

                if (result.Caption != null)
                {
                    writer.WriteLine(" Caption:");
                    writer.WriteLine($"   \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
                }

                if (result.DenseCaptions != null)
                {
                    writer.WriteLine(" Dense Captions:");
                    foreach (var caption in result.DenseCaptions)
                    {
                        writer.WriteLine($"   \"{caption.Content}\", Bounding box {caption.BoundingBox}, Confidence {caption.Confidence:0.0000}");
                    }
                }

                if (result.Objects != null)
                {
                    writer.WriteLine(" Objects:");
                    foreach (var detectedObject in result.Objects)
                    {
                        writer.WriteLine($"   \"{detectedObject.Name}\", Bounding box {detectedObject.BoundingBox}, Confidence {detectedObject.Confidence:0.0000}");
                    }
                }

                if (result.Tags != null)
                {
                    writer.WriteLine($" Tags:");
                    foreach (var tag in result.Tags)
                    {
                        writer.WriteLine($"   \"{tag.Name}\", Confidence {tag.Confidence:0.0000}");
                    }
                }

                if (result.People != null)
                {
                    writer.WriteLine($" People:");
                    foreach (var person in result.People)
                    {
                        writer.WriteLine($"   Bounding box {person.BoundingBox}, Confidence {person.Confidence:0.0000}");
                    }
                }

                if (result.CropSuggestions != null)
                {
                    writer.WriteLine($" Crop Suggestions:");
                    foreach (var cropSuggestion in result.CropSuggestions)
                    {
                        writer.WriteLine($"   Aspect ratio {cropSuggestion.AspectRatio}: "
                            + $"Crop suggestion {cropSuggestion.BoundingBox}");
                    };
                }

                if (result.Text != null)
                {
                    writer.WriteLine($" Text:");
                    foreach (var line in result.Text.Lines)
                    {
                        string pointsToString = "{" + string.Join(',', line.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                        writer.WriteLine($"   Line: '{line.Content}', Bounding polygon {pointsToString}");

                        foreach (var word in line.Words)
                        {
                            pointsToString = "{" + string.Join(',', word.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                            writer.WriteLine($"     Word: '{word.Content}', Bounding polygon {pointsToString}, Confidence {word.Confidence:0.0000}");
                        }
                    }
                }

                var resultDetails = ImageAnalysisResultDetails.FromResult(result);
                writer.WriteLine($" Result details:");
                writer.WriteLine($"   Image ID = {resultDetails.ImageId}");
                writer.WriteLine($"   Result ID = {resultDetails.ResultId}");
                writer.WriteLine($"   Connection URL = {resultDetails.ConnectionUrl}");
                writer.WriteLine($"   JSON result = {resultDetails.JsonResult}");


                writer.WriteLine("text lining:");


                string source = resultDetails.JsonResult.ToString();
                JsonTextReader jtr = new JsonTextReader(new StringReader(source));

                while (jtr.Read())
                {
                    if (jtr.Value != null)
                    {
                        

                        if (jtr.TokenType == JsonToken.String)
                        {
                            writer.WriteLine(jtr.Value);
                        }
                        //_Logger.Info(String.Format("Token {0}, Value: {1}", jtr.TokenType, jtr.Value));
                    }
                    else
                    {
                        //Console.WriteLine("Token: {0}", jtr.TokenType);
                    }
                }

                //new only OCR text result
                string returnFileName = timeOCRStr + "_AICV_OCR_Result_OnlyText.txt";
                StreamWriter writerOnlyText = new StreamWriter(returnFileName);
                JsonTextReader jtrOnlyText = new JsonTextReader(new StringReader(source));

                bool readString = false;
                while(jtrOnlyText.Read())
                {
                    if(jtrOnlyText.Value != null)
                    {
                        //_Logger.Info(String.Format("Token {0}, Value: {1}", jtrOnlyText.TokenType, jtrOnlyText.Value));
                        if (jtrOnlyText.TokenType == JsonToken.String)
                        {
                            string jtrValue = jtrOnlyText.Value.ToString();
                            if (jtrValue.CompareTo("TextElements") != 0)
                            {
                                writerOnlyText.WriteLine(jtrValue);
                                readString = true;
                            }
                            
                        }
                    }
                    else
                    {

                    }
                    if (jtrOnlyText.TokenType == JsonToken.PropertyName &&
                        readString)
                    {
                        jtrOnlyText.Close();
                        writerOnlyText.Close();
                        break;
                    }
                }

                return returnFileName;

            }
            else // result.Reason == ImageAnalysisResultReason.Error
            {
                var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
                writer.WriteLine(" Analysis failed.");
                writer.WriteLine($"   Error reason : {errorDetails.Reason}");
                writer.WriteLine($"   Error code : {errorDetails.ErrorCode}");
                writer.WriteLine($"   Error message: {errorDetails.Message}");
                writer.WriteLine(" Did you set the computer vision endpoint and key?");

                return "Error occured - please refer to result file...";
            }
        }
    }
}
