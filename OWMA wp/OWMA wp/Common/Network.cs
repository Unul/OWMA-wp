using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization.Json;

namespace OWMA_wp.Common
{
    class Network
    {
        private static string ApiUrl = "http://dev-api.owma.ovh";

        public static async Task<HttpResponseMessage> Get(String url, bool authenticate = false)
        {
            try
            {
                HttpClient httpClient = getClient(authenticate);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                authChecker(response);
                return response;
            }
            catch (Exception e)
            {
                Utils.Notify("Une erreur s'est produite", e.Message);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> Post(String url, string Json, bool authenticate = false)
        {
            try
            {
                HttpClient httpClient = getClient(authenticate);
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json"));
                authChecker(response);
                return response;
            }
            catch (Exception e)
            {
                Utils.Notify("Une erreur s'est produite", e.Message);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> Patch(String url, string Json, bool authenticate = false)
        {
            try
            {
                HttpMethod method = new HttpMethod("PATCH");
                HttpClient httpClient = getClient(authenticate);
                HttpContent content = new StringContent(Json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(method, url) { Content = content };
                HttpResponseMessage response = await httpClient.SendAsync(request);
                authChecker(response);
                return response;
            }
            catch (Exception e)
            {
                Utils.Notify("Une erreur s'est produite", e.Message);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> Delete(String url, bool authenticate = false)
        {
            try
            {
                HttpClient httpClient = getClient(authenticate);
                HttpResponseMessage response = await httpClient.DeleteAsync(url);
                authChecker(response);
                return response;
            }
            catch (Exception e)
            {
                Utils.Notify("Une erreur s'est produite", e.Message);
            }
            return null;
        }


        public static T Deserialize<T>(string json)
        {
            Debug.WriteLine(json);
            var _Bytes = Encoding.Unicode.GetBytes(json);
            using (MemoryStream _Stream = new MemoryStream(_Bytes))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(T));
                return (T)_Serializer.ReadObject(_Stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                _Stream.Position = 0;
                using (StreamReader _Reader = new StreamReader(_Stream))
                { return _Reader.ReadToEnd(); }
            }
        }

        public static HttpClient getClient(bool authenticate)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (authenticate)
            {
                httpClient.DefaultRequestHeaders.Add("access-token", CurrentAuthUser.access_token);
                httpClient.DefaultRequestHeaders.Add("client", CurrentAuthUser.client);
                httpClient.DefaultRequestHeaders.Add("expiry", CurrentAuthUser.expiry);
                httpClient.DefaultRequestHeaders.Add("uid", CurrentAuthUser.uid);
            }

            return httpClient;
        }

        public static void authChecker(HttpResponseMessage response)
        {
            Dictionary<string, string> headers = response.Headers.Distinct().ToDictionary(item => item.Key, item => item.Value.First());

            if (headers.ContainsKey("access-token"))
            {
                CurrentAuthUser.access_token = headers["access-token"];
                CurrentAuthUser.client = headers["client"];
                CurrentAuthUser.expiry = headers["expiry"];
                CurrentAuthUser.uid = headers["uid"];
            }
        }
    }
}
