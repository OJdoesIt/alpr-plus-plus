using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.Common
{
    internal static class BetaAuthAPI
    {
        internal static bool IsBetaVersion = true;

        private static string AuthenticationEndpoint = "http://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkAuthorized&fileId={0}&rand={1}&betaKey={2}&textOnly=true";

        public static async Task<bool> IsValidKey(string betaKey)
        {
            string fileSecretUUID = "26cb7dc4-f777-4848-996a-733ebbed9000";

            string rand = System.Guid.NewGuid().ToString();
            string result = "";

            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(string.Format(AuthenticationEndpoint, Constants.LCPDFRDownloadID, rand, betaKey));
            }
            string[] resultArray = result.Split('|');
            if (resultArray[0] != "SUCCESS")
            {
                //Console.WriteLine(String.Format("Authentication failed: Non-standard response -- {0}", resultArray[0]));
                return false;
            }
            int userId = Convert.ToInt32(resultArray[1]);
            string rxChecksum = resultArray[2];
            string expectedChecksum = await ComputeChecksum(resultArray[0], userId, rand, betaKey, fileSecretUUID);
            if (rxChecksum != expectedChecksum)
            {
                //Console.WriteLine(String.Format("Authentication failed: Checksum didn't match {0} != {1}", rxChecksum, expectedChecksum));
                return false;
            }
            //Console.WriteLine(String.Format("Authenticated as LCPDFR.com user ID {0}", userId));
            return true;
        }

        private static async Task<string> ComputeChecksum(string response, int userId, string rand, string apiKey, string fileSecretUUID)
        {
            return await Task.Run(() =>
            {
                System.Security.Cryptography.SHA256Managed crypto = new System.Security.Cryptography.SHA256Managed();
                byte[] hashValue = crypto.ComputeHash(System.Text.Encoding.ASCII.GetBytes(String.Format("{0}{1}{2}{3}{4}", response, userId.ToString(), rand, apiKey, fileSecretUUID)));
                return System.BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            });
        }
    }
}
