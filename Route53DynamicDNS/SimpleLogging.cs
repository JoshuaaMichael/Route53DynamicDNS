using System;
using System.IO;

namespace Route53DynamicDNS
{
    public static class SimpleLogging
    {
        static string defaultLogFilePath = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        static string pendingLogData = "";

        public static void WriteToLog(string text, bool caching = false)
        {
            if (text != "")
            {
                pendingLogData += DateTime.Now.ToString("s") + " - " + text + Environment.NewLine;
            }

            if (caching)
            {
                return;
            }

            File.AppendAllText(defaultLogFilePath, pendingLogData);

            if (Environment.UserInteractive)
            {
                Console.WriteLine(pendingLogData);
            }

            pendingLogData = "";
        }

        public static void WipePendingLogData()
        {
            pendingLogData = "";
        }
    }
}
