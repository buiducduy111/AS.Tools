using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// Http request helper
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Accept
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// Accept Language
        /// </summary>
        public string AcceptLanguage { get; set; }

        /// <summary>
        /// Security type
        /// </summary>
        public SecurityProtocolType SecurityProtocolType { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// Init
        /// </summary>
        public HttpRequest()
        {
            this.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Safari/537.36";
            this.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            this.AcceptLanguage = "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5,ja;q=0.4";
            this.SecurityProtocolType = SecurityProtocolType.Tls12;
            this.CookieContainer = new CookieContainer();
        }

        /// <summary>
        /// Get string from a http request. Return html or REQUEST_ERROR_
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authUser"></param>
        /// <param name="authPass"></param>
        /// <param name="useSSL"></param>
        ///  <param name="useCookie"></param>
        /// <returns></returns>
        public string GetString(string url, string authUser = "", string authPass = "", bool useSSL = false, bool useCookie = true)
        {
            if (useSSL)
                ServicePointManager.SecurityProtocol = this.SecurityProtocolType;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
            };

            if (useCookie)
            {
                handler.CookieContainer = this.CookieContainer;
                handler.UseCookies = true;
            }

            if (!String.IsNullOrEmpty(authUser))
            {
                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(url), "Basic", new NetworkCredential(authUser, authPass));
                handler.Credentials = credentialCache;
            }

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                client.DefaultRequestHeaders.TryAddWithoutValidation("accept", this.Accept);
                client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", this.AcceptLanguage);

                try
                {
                    string html = client.GetStringAsync(url).Result;
                    client.Dispose();

                    return html;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    if (ex.InnerException != null) error += " " + ex.InnerException.Message;

                    return "REQUEST_ERROR_" + error;
                }
            }
        }

        /// <summary>
        /// Get string from a http request, using Proxy
        /// </summary>
        /// <param name="url"></param>
        /// <param name="proxyIP"></param>
        /// <param name="proxyPort"></param>
        /// <param name="proxyUser"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useSSL"></param>
        /// <returns></returns>
        public string GetString(string url, string proxyIP, int proxyPort, string proxyUser = "", string proxyPassword = "", bool useSSL = false)
        {
            if (useSSL)
                ServicePointManager.SecurityProtocol = this.SecurityProtocolType;

            
            WebProxy proxy = new WebProxy(proxyIP, proxyPort);
            if (String.IsNullOrEmpty(proxyUser))
            {
                proxy.UseDefaultCredentials = false;
                proxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPassword);
            }

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
                UseProxy = true,
                Proxy = proxy
            };


            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                client.DefaultRequestHeaders.Add("accept", this.Accept);
                client.DefaultRequestHeaders.Add("accept-language", this.AcceptLanguage);

                try
                {
                    string html = client.GetStringAsync(url).Result;
                    client.Dispose();

                    return html;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    if (ex.InnerException != null) error += " " + ex.InnerException.Message;

                    return "REQUEST_ERROR_" + error;
                }
            }
        }

        /// <summary>
        /// Post string from a http request. Return html or REQUEST_ERROR_
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="authUser"></param>
        /// <param name="authPass"></param>
        /// <param name="useSSL"></param>
        ///  <param name="useCookie"></param>
        /// <returns></returns>
        public string Post(string url, string data, string authUser = "", string authPass = "", bool useSSL = false, bool useCookie = true)
        {
            if (useSSL)
                ServicePointManager.SecurityProtocol = this.SecurityProtocolType;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
            };

            if (useCookie)
            {
                handler.CookieContainer = this.CookieContainer;
                handler.UseCookies = true;
            }

            if (!String.IsNullOrEmpty(authUser))
            {
                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(url), "Basic", new NetworkCredential(authUser, authPass));
                handler.Credentials = credentialCache;
            }

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                client.DefaultRequestHeaders.TryAddWithoutValidation("accept", this.Accept);
                client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", this.AcceptLanguage);

                try
                {
                    StringContent content = new StringContent(data);

                    string html = client.PostAsync(url, content).Result.
                        Content.ReadAsStringAsync().Result;

                    client.Dispose();

                    return html;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    if (ex.InnerException != null) error += " " + ex.InnerException.Message;

                    return "REQUEST_ERROR_" + error;
                }
            }
        }


        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="url"></param>
        /// <param name="outputFile"></param>
        /// <param name="msg"></param>
        /// <param name="useSSL"></param>
        /// <returns></returns>
        public bool DownloadFile(string url, string outputFile, out string msg, bool useSSL = false)
        {
            if (useSSL)
                ServicePointManager.SecurityProtocol = this.SecurityProtocolType;

            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Accept, "image/jpg");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,vi;q=0.8");
                client.Headers.Add(HttpRequestHeader.UserAgent, this.UserAgent);

                try
                {
                    client.DownloadFile(url, outputFile);
                    msg = "Success";
                    return true;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    if (ex.InnerException != null) error += " " + ex.InnerException.Message;
                    msg = error;
                    return false;
                }
            }
        }
    }
}
