using System;
using System.IO;
using Newtonsoft.Json;

namespace Route53DynamicDNS
{
    public static class ServiceConfig
    {
        internal class ServiceConfigData
        {
            public int GetPublicIPTries;
            public string AwsAccessKeyId;
            public string AwsSecretAccessKey;
            public string DomainName;
            public string HostedZoneId;
            public int AWSGetChangeSleepTime;
            public int DnyDNSPollTime;
            public bool WriteLogsOnlyIfIPChanged;

            public bool BasicValidate()
            {
                if (GetPublicIPTries <= 0) { return false; }
                if (AwsAccessKeyId.Trim().Length == 0) { return false; }
                if (AwsSecretAccessKey.Trim().Length == 0) { return false; }
                if (DomainName.Trim().Length == 0) { return false; }
                if (HostedZoneId.Trim().Length == 0) { return false; }
                if (AWSGetChangeSleepTime <= 0) { return false; }
                if (DnyDNSPollTime <= 0) { return false; }

                return true;
            }
        }

        private static ServiceConfigData serviceConfigData;

        public static void Load()
        {
            //Declared here so it's not serialised into file
            string defaultConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "config.json";

            try
            {
                SimpleLogging.WriteToLog("Opening config file: " + defaultConfigFilePath);
                string configJson = File.ReadAllText(defaultConfigFilePath);
                serviceConfigData = JsonConvert.DeserializeObject<ServiceConfigData>(configJson);
            }
            catch
            {
                string message = "Issue opening config file; please put config.json file in same directory as .exe service";
                SimpleLogging.WriteToLog(message);
                throw new Exception(message);
            }

            if (!serviceConfigData.BasicValidate())
            {
                string message = "Invalid parameter in config.json file";
                SimpleLogging.WriteToLog(message);
                throw new Exception(message);
            }
        }

        public static int GetPublicIPTries { get { return serviceConfigData.GetPublicIPTries; } }
        public static string AwsAccessKeyId { get { return serviceConfigData.AwsAccessKeyId; } }
        public static string AwsSecretAccessKey { get { return serviceConfigData.AwsSecretAccessKey; } }
        public static string DomainName { get { return serviceConfigData.DomainName; } }
        public static string HostedZoneId { get { return serviceConfigData.HostedZoneId; } }
        public static int AWSGetChangeSleepTime { get { return serviceConfigData.AWSGetChangeSleepTime; } }
        public static int DnyDNSPollTime { get { return serviceConfigData.DnyDNSPollTime; } }
        public static bool WriteLogsOnlyIfIPChanged { get { return serviceConfigData.WriteLogsOnlyIfIPChanged; } }
    }
}
