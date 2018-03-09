using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Amazon;
using Amazon.Route53;
using Amazon.Route53.Model;

namespace Route53DynamicDNS
{
    public static class Route53RecordUpdater
    {
        static string lastIP = "";

        //https://docs.aws.amazon.com/sdk-for-net/v2/developer-guide/route53-apis-intro.html
        public static void UpdateRecord()
        {
            //TODO: Better exception handling overall in the method
            SimpleLogging.WriteToLog("Time to check for update to public IP", true);
            SimpleLogging.WriteToLog("Last public IP was " + lastIP, true);
            string publicIP = GetPublicIP(ServiceConfig.GetPublicIPTries);
            SimpleLogging.WriteToLog("Current public IP is " + publicIP, true);

            if (publicIP == "")
            {
                SimpleLogging.WriteToLog("Failed to check for new public IP");
                return;
            }

            if (publicIP == lastIP)
            {
                if (ServiceConfig.WriteLogsOnlyIfIPChanged)
                {
                    SimpleLogging.WipePendingLogData();
                }
                else
                {
                    SimpleLogging.WriteToLog("Public IP address has not changed" + Environment.NewLine);
                }
                return;
            }
            SimpleLogging.WriteToLog("Contacting AWS to update public IP");
            lastIP = publicIP;

            //TODO: Better exception handling when these values aren't working
            //TODO: Confirm all users can use the APSoutheast2 endpoint
            var route53Client = new AmazonRoute53Client(ServiceConfig.AwsAccessKeyId, ServiceConfig.AwsSecretAccessKey, RegionEndpoint.APSoutheast2);

            SimpleLogging.WriteToLog("Successfully authenticated to AWS");

            var recordSet = new ResourceRecordSet()
            {
                Name = ServiceConfig.DomainName,
                TTL = 60,
                Type = RRType.A, //TODO: Update so can do IPv6 records also where required
                ResourceRecords = new List<ResourceRecord>
                {
                  new ResourceRecord { Value = publicIP }
                }
            };

            var changeBatch = new ChangeBatch()
            {
                Changes = new List<Change>
                {
                    new Change()
                    {
                        ResourceRecordSet = recordSet,
                        Action = ChangeAction.UPSERT
                    }
                }
            };

            var recordsetRequest = new ChangeResourceRecordSetsRequest()
            {
                HostedZoneId = ServiceConfig.HostedZoneId,
                ChangeBatch = changeBatch
            };

            var recordsetResponse = route53Client.ChangeResourceRecordSets(recordsetRequest);

            SimpleLogging.WriteToLog("Succesfully submitted update request to AWS");

            var changeRequest = new GetChangeRequest()
            {
                Id = recordsetResponse.ChangeInfo.Id
            };

            while (ChangeStatus.PENDING == route53Client.GetChange(changeRequest).ChangeInfo.Status)
            {
                SimpleLogging.WriteToLog("Waiting for change to be INSYNC");
                Thread.Sleep(ServiceConfig.AWSGetChangeSleepTime);
            }

            SimpleLogging.WriteToLog("Succesfully updated IP address to " + publicIP + Environment.NewLine);
        }

        private static string GetPublicIP(int tries)
        {
            //TODO: Exception handling of various errors that can occur
            //TODO: Confirm how IPv6 is handled
            string publicIP = "";
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    publicIP = new WebClient().DownloadString("http://checkip.amazonaws.com/");
                    publicIP = publicIP.Trim();
                }
                catch { } //Do nothing, just don't break

                if (publicIP != "")
                {
                    return publicIP;
                }
            }
            return "";
        }

    }
}
