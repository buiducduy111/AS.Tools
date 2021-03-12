using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    public class Server
    {
        private string _key;
        private string _server;

        /// <summary>
        /// Init with Key and server domain
        /// </summary>
        /// <param name="key"></param>
        /// <param name="server"></param>
        public Server(string key, string server = "http://codethuegiare.com")
        {
            _key = key;
            _server = server;
        }

        /// <summary>
        /// Check license
        /// </summary>
        /// <param name="msg">Out message</param>
        /// <param name="paramFuncs">Set param funcs</param>
        /// <returns></returns>
        public bool CheckLicense(out string msg, out string paramFuncs)
        {
            paramFuncs = "";

            string cpuid = SystemInfo.GetCpuID();
            string machineName = Environment.MachineName;

            string url = _server + $"/api/client/checkjobkey?key={_key}&macAddress={cpuid}&machinename={machineName}";

            try
            {
                string result = httpGet(url);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(result);

                msg = Convert.ToString(obj.msg);
                paramFuncs = Convert.ToString(obj.param_funcs);
                return Convert.ToInt16(obj.status) == 200;
            }
            catch
            {
                msg = "Lỗi mạng hoặc không tìm thấy server. Vui lòng thử lại!";
                return false;
            }
        }


        /// <summary>
        /// Post domain log
        /// </summary>
        /// <param name="scanUrl"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public string PostDomain(string scanUrl, string platform)
        {
            string url = _server + $"/api/client/log?key={_key}&scanUrl={scanUrl}&platform={platform}";

            try
            {
                return httpGet(url);
            }
            catch (Exception ex)
            {
                return "ERROR_" + ex.Message;
            }
        }

        /// <summary>
        /// Check update
        /// </summary>
        /// <returns></returns>
        public bool HasUpdate(string currentVersion)
        {
            string url = _server + $"/api/client/getfileversion?key={_key}";

            try
            {
                string result = httpGet(url);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(result);

                if (Convert.ToInt16(obj.status) == 200)
                {
                    string version = Convert.ToString(obj.version);
                    return version != currentVersion;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        #region Request
        private static string httpGet(string url)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            try
            {

                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                Stream stream = res.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string html = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                res.Close();
                request.Abort();
                return html;
            }
            catch (Exception ex)
            {
                request.Abort();
                return "REQUEST_ERROR_" + ex.Message;
            }
        }
        #endregion
    }
}
