using System.IO;
using System.Net;

namespace E_Shop_Engine.Utilities
{
    public static class RequestWeb
    {
        public static string GetOperationDetails(string operation_number)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ssl.dotpay.pl/test_seller/api/v1/operations/" + operation_number + "/");
            request.Credentials = new NetworkCredential("ladydeath@o2.pl", "Qwerty1!");
            request.Host = "ssl.dotpay.pl";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
    }
}
