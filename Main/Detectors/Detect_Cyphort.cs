using Fido_Main.Fido_Support.Objects.Fido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fido_Main.Main.Detectors
{
    public class Detect_Cyphort
    {
        public string Message { get; set; }
        public string Config { get; set; }

        public int AlertType { get; set; }

        public static void GetCyphortAlerts()
        {
            Console.WriteLine(@"Running Cyphort {Message} detector.");
            //currently needed to bypass site without a valid cert.
            //todo: make ssl bypass configurable
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var parseConfigs = Object_Fido_Configs.ParseDetectorConfigs("cyphortv2");
            var request = parseConfigs.Server + parseConfigs.Query + parseConfigs.APIKey;
            var alertRequest = (HttpWebRequest)WebRequest.Create(request);
            alertRequest.Method = "GET";
            try
            {
                using (var cyphortResponse = alertRequest.GetResponse() as HttpWebResponse)
                {
                    if (cyphortResponse != null && cyphortResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var respStream = cyphortResponse.GetResponseStream())
                        {
                            if (respStream == null) return;
                            var cyphortReader = new StreamReader(respStream, Encoding.UTF8);
                            var stringreturn = cyphortReader.ReadToEnd();
                            var cyphortReturn = JsonConvert.DeserializeObject<CyphortClass>(stringreturn);
                            if (cyphortReturn.correlations_array.Any() | cyphortReturn.infections_array.Any() | cyphortReturn.downloads_array.Any())
                            {
                                ParseCyphort(cyphortReturn);
                            }
                            var responseStream = cyphortResponse.GetResponseStream();
                            if (responseStream != null) responseStream.Dispose();
                            cyphortResponse.Close();
                            Console.WriteLine(@"Finished processing Cyphort detector.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Cyphort Detector getting json:" + e);
            }
        }

     


        public static void ALERT_3()
        {
            Console.WriteLine(@"Running Cyphort v3 detector.");
            //currently needed to bypass site without a valid cert.
            //todo: make ssl bypass configurable
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var parseConfigs = Object_Fido_Configs.ParseDetectorConfigs("cyphortv3");
            var request = parseConfigs.Server + parseConfigs.Query + parseConfigs.APIKey;
            var alertRequest = (HttpWebRequest)WebRequest.Create(request);
            alertRequest.Method = "GET";
            try
            {
                using (var cyphortResponse = alertRequest.GetResponse() as HttpWebResponse)
                {
                    if (cyphortResponse != null && cyphortResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var respStream = cyphortResponse.GetResponseStream())
                        {
                            if (respStream == null) return;
                            var cyphortReader = new StreamReader(respStream, Encoding.UTF8);
                            var stringreturn = cyphortReader.ReadToEnd();
                            var cyphortReturn = JsonConvert.DeserializeObject<Object_Cyphort_Class.CyphortEvent>(stringreturn);
                            if (cyphortReturn.Event_Array.Any())
                            {
                                ParseCyphort(cyphortReturn);
                            }
                            var responseStream = cyphortResponse.GetResponseStream();
                            if (responseStream != null) responseStream.Dispose();
                            cyphortResponse.Close();
                            Console.WriteLine(@"Finished processing Cyphort detector.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Cyphort Detector getting json:" + e);
            }
        }

    }
}
