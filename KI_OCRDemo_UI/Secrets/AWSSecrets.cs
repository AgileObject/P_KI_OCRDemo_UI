using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using log4net;

namespace KI_OCRDemo_UI.AWS_Textract_Secrets
{
    public class AWSSecrets
    {
        private static readonly ILog _Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // These are the names of the environment variables you can set if you want to specify
        // secrets using environment variables instead of command-line arguments
        public static readonly string EnvironmentVariableKey = "AWS_ACCESS_KEYID";
        public static readonly string EnvironmentVariableEndpoint = "AWS_SECRET_ACCESS_KEY";

        // Get these properties only after a call to LoadSucceed returned true
        public static string AccessKeyId { get; private set; }
        public static string SecretAccessKey { get; private set; }

        public static bool LoadSucceeded(string[] args)
        {
            bool loadSucceeded = LoadKeySucceeded(args) && LoadEndpointSucceeded(args);

            if (loadSucceeded)
            {
                // Do not print the full value of your Computer Vision key to the console (or log file).
                // Here we only log the last 3 characters of the key.
                string maskedAccessKey = new String('*', 16) + AccessKeyId.Substring(16, 4);
                string maskedSecretAccessKey = new String('*', 36) + SecretAccessKey.Substring(36, 4);

               _Logger.Info(" Using AWS Access KeyId: " + maskedAccessKey);
               _Logger.Info(" Using AWS Secret Access Key: " + maskedSecretAccessKey);
            }

            return loadSucceeded;
        }

        private static bool LoadKeySucceeded(string[] args)
        {
            // First try to read vision key from command-line arguments
            if (args != null && args.Length > 0)
            {
                foreach (string flag in new string[] { "--key", "-k" })
                {
                    int i = Array.IndexOf(args, flag);
                    if (i >= 0 && i < args.Length - 1)
                    {
                        AccessKeyId = args[i + 1];
                        break;
                    }
                }
            }

            // If not found, try to read it from environment variable
            if (AccessKeyId == null)
            {
                try
                {
                    AccessKeyId = Environment.GetEnvironmentVariable(EnvironmentVariableKey);
                }
                catch
                {
                    // Ignore
                }
            }

            // If not found, try to read it from the app.config
            if (AccessKeyId == null)
            {
                try
                {
                    AccessKeyId = ConfigurationManager.AppSettings["AccessKeyId"].ToString();
                }
                catch
                {
                    // Ignore
                }

            }

            return (!String.IsNullOrEmpty(AccessKeyId));
        }

        // Returns true of the Computer Vision Endpoint URL exists and is valid. False otherwise.
        private static bool LoadEndpointSucceeded(string[] args)
        {
            // First try to read endpoint from command-line arguments
            if (args != null && args.Length > 0)
            {
                foreach (string flag in new string[] { "--endpoint", "-e" })
                {
                    int i = Array.IndexOf(args, flag);
                    if (i >= 0 && i < args.Length - 1)
                    {
                        SecretAccessKey = args[i + 1];
                        break;
                    }
                }
            }

            // If not found, try to read it from environment variable
            if (SecretAccessKey == null)
            {
                try
                {
                    SecretAccessKey = Environment.GetEnvironmentVariable(EnvironmentVariableEndpoint);
                }
                catch
                {
                    // Ignore
                }
            }

            if (SecretAccessKey == null)
            {
                try
                {
                    SecretAccessKey = ConfigurationManager.AppSettings["SecretAccessKey"].ToString();
                }
                catch
                {
                    // Ignore
                }
            }

            //return IsValidEndpoint(SecretAccessKey);
            return (!String.IsNullOrEmpty(SecretAccessKey));
        }

        // Validates the format of the Computer Vision Endpoint URL.
        // Returns true if the endpoint is valid, false otherwise.
        //private static bool IsValidEndpoint(string SecretAccessKey)
        //{
        //    if (String.IsNullOrWhiteSpace(SecretAccessKey))
        //    {
        //        Console.WriteLine(" Error: Missing computer vision endpoint.");
        //        Console.WriteLine("");
        //        return false;
        //    }

        //    if (!Regex.IsMatch(SecretAccessKey, @"^https://\S+\.cognitiveservices\.azure\.com/?$"))
        //    {
        //        Console.WriteLine($" Error: Invalid value for computer vision endpoint: {endpoint}.");
        //        Console.WriteLine(" It should be in the form: https://<your-computer-vision-resource-name>.cognitiveservices.azure.com");
        //        Console.WriteLine("");
        //        return false;
        //    }

        //    return true;
        //}

        // Validates the format of the Computer Vision Key.
        // Returns true if the key is valid, false otherwise.
        private static bool IsValidKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine(" Error: Missing computer vision key.");
                Console.WriteLine("");
                return false;
            }

            if (!Regex.IsMatch(key, @"^[a-fA-F0-9]{32}$"))
            {
                _Logger.Info($" Error: Invalid value for computer vision key: {key}.");
                _Logger.Info(" It should be a 32-character HEX number.");
                _Logger.Info("");
                return false;
            }

            return true;
        }

    }
}