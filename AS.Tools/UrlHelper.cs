using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// Helper for URL
    /// </summary>
    public class UrlHelper
    {
        static Random _rand = new Random();

        /// <summary>
        /// Get domain from a url. Return eg: http://codethuegiare.com
        /// </summary>
        /// <param name="url">Eg: https://giaiphapmmo.net/download?active_key=ABC</param>
        /// <returns></returns>
        public static string GetDomain(string url)
        {
            Uri uri = new Uri(url);
            return uri.Scheme + "://" + uri.Host;
        }

        /// <summary>
        /// Get slug from url. Eg: https://abc.com/products/duy-dep-trai/?action=true => duy-dep-trai
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSlug(string url)
        {
            try
            {
                Uri uri = new Uri(url);

                // Get path. Eg: /products/duy-dep-trai/
                string path = uri.LocalPath;
                if (path[path.Length - 1] == '/') path = path.Substring(0, path.Length - 1);

                return path.Substring(path.LastIndexOf('/') + 1);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Make slug from a title. Eg: Duy Dep Trai => duy-dep-trai
        /// </summary>
        /// <param name="title"></param>
        /// <param name="addRandomNumber">If true, add a random number to last result (1000 to 10000)</param>
        /// <returns></returns>
        public static string MakeSlug(string title, bool addRandomNumber = false)
        {
            string result = "";
            string raw = title.Trim().ToLower().Replace("  ", " ");

            // Allow only a-z (97 to 122) or Space (32) or 0-9 (48-57)
            for (int i = 0; i < raw.Length; i++)
            {
                int iValue = (int)raw[i];

                if ((iValue >= 97 && iValue <= 122) || (iValue >= 48 && iValue <= 57))
                    result += raw[i];

                if (iValue == 32)
                    result += "-";
            }

            if (addRandomNumber)
                result += "-" + _rand.Next(1000, 10000);

            return result;
        }

    }
}
