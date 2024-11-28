using log4net;
using log4net.Config;
using System.Reflection;

namespace KI_OCRDemo_UI
{
    internal static class Program
    {
        private static ILog _Logger;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            InitializeLogging();
            ApplicationConfiguration.Initialize();
            Application.Run(new mainFormOCRDemo());
        }

        private static void InitializeLogging()
        {
            //log4net initializing
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            string assemblyPath = fi.DirectoryName;
            string fileNameLog4Net = Path.Combine(assemblyPath, "log4net.config");
            XmlConfigurator.Configure(logRepository, new FileInfo(fileNameLog4Net));

            _Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}