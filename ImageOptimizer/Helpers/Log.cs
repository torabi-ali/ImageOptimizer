using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;

namespace ImageOptimizer.Helpers
{
    public static class LogException
    {
        private static string fileName = $"{Properties.Settings.Default.DefaultPath}{Application.ResourceAssembly.GetName().Name}.log";

        public static void Log(this Exception ex)
        {
            StringBuilder error = new StringBuilder();

            error.AppendLine("Date:                       " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            error.AppendLine("Computer name:              " + Environment.MachineName);
            error.AppendLine("User name:                  " + Environment.UserName);
            error.AppendLine("OS:                         " + Environment.OSVersion.ToString());
            error.AppendLine("Culture:                    " + CultureInfo.CurrentCulture.Name);
            error.AppendLine("App up time:                " + (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());
            error.AppendLine("");

            error.AppendLine("Exception Message:          " + ex.Message);
            error.AppendLine("");

            error.AppendLine("Exception Source:           " + ex.Source);
            error.AppendLine("");

            error.AppendLine("Exception HelpLink:         " + ex.HelpLink);
            error.AppendLine("");

            error.AppendLine("Exception TargetSite:       " + ex.TargetSite);
            error.AppendLine("");

            error.AppendLine("Exception InnerException:   " + ex.InnerException);
            error.AppendLine("");

            error.AppendLine("Stack StackTrace:           " + ex.StackTrace);
            error.AppendLine("");

            error.AppendLine("Loaded Modules:");
            Process thisProcess = Process.GetCurrentProcess();
            foreach (ProcessModule module in thisProcess.Modules)
            {
                error.AppendLine("\t" + module.FileName + " " + module.FileVersionInfo.FileVersion);
            }
            error.AppendLine("\n- - - - - - - - - - * * * * * * * * * - - - - - - - - - -\n");
            error.Save();
        }

        public static void Save(this StringBuilder error)
        {
            File.AppendAllText(fileName, error.ToString(), Encoding.UTF8);
        }
    }
}
