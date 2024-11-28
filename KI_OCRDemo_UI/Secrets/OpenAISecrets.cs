using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KI_OCRDemo_UI.OPENAI_Secrets
{
    public class OpenAISecrets
    {
        private static readonly ILog _Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly string EnvironmentVariableKey = "OPENAI_API_KEY";
        public static readonly string EnvironmentVariableEndpoint = "OPENAI_ BASE_ADRESS";

        // Get these properties only after a call to LoadSucceed returned true
        public static string APIKey { get; private set; }
        public static string BaseAddress { get; private set; }

        public static bool LoadSucceeded(string[] args)
        {
            bool loadSucceeded = LoadAPIKeySucceeded(args) && LoadBaseAddressSucceeded(args);

            if (loadSucceeded)
            {
                // Do not print the full value of your Computer Vision key to the console (or log file).
                // Here we only log the last 3 characters of the key.
                string maskedAPIKey = new String('*', 16) + APIKey.Substring(16, 4);
                //string maskedSecretAccessKey = new String('*', 36) + BaseAddress.Substring(36, 4);

                _Logger.Info(" Using OpenAI APIKey: " + maskedAPIKey);
                _Logger.Info(" Using OpenAI BaseAddress: " + BaseAddress);
            }

            return loadSucceeded;
        }

        private static bool LoadAPIKeySucceeded(string[] args)
        {
            // First try to read vision key from command-line arguments
            if (args != null && args.Length > 0)
            {
                foreach (string flag in new string[] { "--APIKey", "-k" })
                {
                    int i = Array.IndexOf(args, flag);
                    if (i >= 0 && i < args.Length - 1)
                    {
                        APIKey = args[i + 1];
                        break;
                    }
                }
            }

            // If not found, try to read it from environment variable
            if (APIKey == null)
            {
                try
                {
                    APIKey = Environment.GetEnvironmentVariable(EnvironmentVariableKey);
                }
                catch
                {
                    // Ignore
                }
            }

            // If not found, try to read it from the app.config
            if (APIKey == null)
            {
                try
                {
                    APIKey = ConfigurationManager.AppSettings["APIKey"].ToString();
                }
                catch
                {
                    // Ignore
                }

            }

            return (!String.IsNullOrEmpty(APIKey));
        }

        // Returns true of the Computer Vision Endpoint URL exists and is valid. False otherwise.
        private static bool LoadBaseAddressSucceeded(string[] args)
        {
            // First try to read endpoint from command-line arguments
            if (args != null && args.Length > 0)
            {
                foreach (string flag in new string[] { "--BaseAddress", "-e" })
                {
                    int i = Array.IndexOf(args, flag);
                    if (i >= 0 && i < args.Length - 1)
                    {
                        BaseAddress = args[i + 1];
                        break;
                    }
                }
            }

            // If not found, try to read it from environment variable
            if (BaseAddress == null)
            {
                try
                {
                    BaseAddress = Environment.GetEnvironmentVariable(EnvironmentVariableEndpoint);
                }
                catch
                {
                    // Ignore
                }
            }

            if (BaseAddress == null)
            {
                try
                {
                    BaseAddress = ConfigurationManager.AppSettings["BaseAddress"].ToString();
                }
                catch
                {
                    // Ignore
                }
            }

            //return IsValidEndpoint(SecretAccessKey);
            return (!String.IsNullOrEmpty(BaseAddress));
        }

    }
}
