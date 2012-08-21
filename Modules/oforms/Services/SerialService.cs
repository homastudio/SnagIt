using System;
using System.IO;
using System.Web;
using Orchard.Logging;

namespace oforms.Services
{
    public class SerialService : ISerialService
    {
        public SerialService() {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void SaveSerialToFile(string value) {
            var serialFilePath = GetSerialFilePath();
            var oformsDataDir = Path.GetDirectoryName(serialFilePath);
            if (!Directory.Exists(oformsDataDir))
            {
                Directory.CreateDirectory(oformsDataDir);
            }

            using (var sw = new StreamWriter(serialFilePath))
            {
                sw.WriteLine(value);
            }
        }
        
        public string ReadSerialFromFile() {
            string text;
            try {
                text = File.ReadAllText(GetSerialFilePath());
            }
            catch (Exception ex)
            {
            	Logger.Information(ex, "Can not read file.");
                return "";
            }
            return text.Trim();
        }

        private string DecodeSerial(string str)
        {
            try
            {
                // ignore 3st 3 letters
                str = str.Substring(3, str.Length - 3);
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch (Exception ex)
            {
            	Logger.Information(ex, "Can not decode serial");
                return "";
            }

        }

        private static bool IsNumeric(string s)
        {
            double Result;
            return double.TryParse(s, out Result);  // TryParse routines were added in Framework version 2.0.
        } 


        public bool ValidateSerial() {
            if (IsNumeric(DecodeSerial(this.ReadSerialFromFile())))
            {
                return true;
            }
            else {
                return false;
            }
        }

        private string GetSerialFilePath()
        {
            var oformsDataDir = HttpContext.Current.Server.MapPath("~/App_Data/oforms/");
            return Path.Combine(oformsDataDir, "sn.dat");
        }
    }
}