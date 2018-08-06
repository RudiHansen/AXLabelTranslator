using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace ConsoleUI
{
    public class Trans
    {
        /// URL of the token service
        private static readonly Uri ServiceUrl = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");

        /// Name of header used to pass the subscription key to the token service
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        /// Gets the HTTP status code for the most recent request to the token service.

        public static string GetAccessToken(string subscriptionKey)
        {
            Trans trans = new Trans();

            var task = trans.GetAccessTokenAsync(subscriptionKey);
            var result = task.Result;

            return result;
        }

        public async Task<string> GetAccessTokenAsync(string subscriptionKey)
        {
            HttpStatusCode RequestStatusCode;
            if (string.IsNullOrEmpty(subscriptionKey))
            {
                return string.Empty;
            }

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = ServiceUrl;
                request.Content = new StringContent(string.Empty);
                request.Headers.TryAddWithoutValidation(OcpApimSubscriptionKeyHeader, subscriptionKey);
                client.Timeout = TimeSpan.FromSeconds(2);
                var response = await client.SendAsync(request);
                RequestStatusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();
                return "Bearer " + token; ;
            }
        }

        public static string Translate(string authToken, string text, string from, string to)
        {
            string retVal = "";
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                retVal = (string)dcs.ReadObject(stream);
            }
            return retVal;
        }

    }
}
