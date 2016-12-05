using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WexinCardCreater.Http
{
    public class HttpHelper
    {
        private static readonly Func<HttpContent, string> StringFunc = hc =>
        {
            var responsejson = hc.ReadAsStringAsync().Result;
            return responsejson;
        };

        /// <summary>
        ///     http get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpRequestGet(string url
        )
        {
            using (var http = new HttpClient())
            {
                http.Timeout = new TimeSpan(0, 0, 0, 30);

                var result = http.GetAsync(url).Result;
                var deresponsejson = StringFunc(result.Content);
                return deresponsejson;
            }
        }

        /// <summary>
        ///     http Post 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postContentJson"></param>
        /// <returns></returns>
        public static string HttpRequestPost(string url, string postContentJson)

        {
            HttpContent httpContent = null;
            if (postContentJson.IsNotNullOrEmpty())
            {
                var requestjson = postContentJson;
                httpContent = new StringContent(requestjson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            using (var http = new HttpClient())
            {
                http.Timeout = new TimeSpan(0, 0, 0, 30);

                var result = http.PostAsync(url, httpContent).Result;
                var deresponsejson = StringFunc(result.Content);

                return deresponsejson;
            }
        }
    }
}