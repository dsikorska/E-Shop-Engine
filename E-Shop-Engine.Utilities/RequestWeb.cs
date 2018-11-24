using System.Configuration;
using System.IO;
using System.Net;

namespace E_Shop_Engine.Utilities
{
    public static class RequestWeb
    {
        /// <summary>
        /// Get operation details from dot pay server.
        /// </summary>
        /// <param name="operation_number">Transaction number set by dot pay.</param>
        /// <returns>Transaction details.</returns>
        public static string GetOperationDetails(string operation_number)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ssl.dotpay.pl/test_seller/api/v1/operations/" + operation_number + "/");
            string login = ConfigurationManager.AppSettings["dotPayLogin"];
            string pw = ConfigurationManager.AppSettings["dotPayPassword"];
            request.Credentials = new NetworkCredential(login, pw);
            request.Host = "ssl.dotpay.pl";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
    }
}
