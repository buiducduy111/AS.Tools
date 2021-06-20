using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// Helper for string
    /// </summary>
    public class StrHelper
    {
        // Local
        static Random _rand = new Random();


        /// <summary>
        /// Get allowed file name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAllowedFileName(string name)
        {
            char[] removeChars = new char[]
            {
                '/','\\','?','%','*',':','|','\"','<','>',';','='
            };

            string result = "";
            for (int i = 0; i < name.Length; i++)
            {
                if (removeChars.Contains(name[i]))
                    continue;

                result += name[i];
            }

            return result;
        }

        /// <summary>
        /// Make a random string includes a-z,A-Z,0-9
        /// </summary>
        /// <param name="lengh"></param>
        /// <returns></returns>
        public static string RandomString(int lengh)
        {
            // Add list accept characters
            List<char> accepChars = new List<char>();
            for (int i = 0; i < 10; i++)
                accepChars.Add(i.ToString()[0]);
            for (int i = 65; i < 91; i++)
                accepChars.Add((char)i);
            for (int i = 97; i < 123; i++)
                accepChars.Add((char)i);

            // Random
            string str = "";

            for (int i = 0; i < lengh; i++)
            {
                str += accepChars[_rand.Next(0, accepChars.Count)];
            }

            return str;
        }

        /// <summary>
        /// Convert a Text to List
        /// </summary>
        /// <param name="text">Eg: tag 1, tag 2, tag 3</param>
        /// <param name="separator">Separtor character. Normaly: ','</param>
        /// <returns></returns>
        public static List<T> ParseList<T>(string text, char separator = ',')
        {
            List<T> result = new List<T>();

            string[] spliter = text.Split(separator);
            foreach (string item in spliter)
            {
                if (String.IsNullOrEmpty(item.Trim())) continue;
                result.Add((T)Convert.ChangeType(item.Trim(), typeof(T)));
            }

            return result;
        }

        /// <summary>
        /// Detect the first number > 0 of a string. Return null if not detected
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int? IntDetect(string text)
        {
            string result = "";
            int tmp;

            for (int i = 0; i < text.Length; i++)
            {
                if (int.TryParse(text[i].ToString(), out tmp)) result += text[i];
                else
                {
                    if (String.IsNullOrEmpty(result))
                        continue;
                    else
                        break;
                }
            }

            if (String.IsNullOrEmpty(result))
                return null;

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Detect first double from a string. Eg: $39.99abc -> 39.99
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double? DoubleDetect(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                // Accept 0-9, '.', ',', '-xx' (-1, -2, -3..)
                if (isInt(text[i].ToString()) || text[i] == '.' || text[i] == ',' ||
                    (i < text.Length - 1 && text[i] == '-' && isInt(text[i + 1])))
                {
                    result += text[i];

                    if (i == text.Length - 1)
                        return Convert.ToDouble(result);
                }
                else
                {
                    if (result != "")
                        return Convert.ToDouble(result.Replace(",", "."));
                }
            }

            return null;
        }

        /// <summary>
        /// Get beetween text from startKey and endKey. Return null if not exists
        /// </summary>
        /// <param name="html"></param>
        /// <param name="startKey"></param>
        /// <param name="endKey"></param>
        /// <returns></returns>
        public static string GetBetweenText(string html, string startKey, string endKey)
        {
            if (html.IndexOf(startKey) < 0)
                return null;

            try
            {
                int startIndex = html.IndexOf(startKey) + startKey.Length;
                int endIndex = html.IndexOf(endKey, startIndex);

                return html.Substring(startIndex, endIndex - startIndex);
            }
            catch
            {
                return null;
            }
        }

        #region Helpers
        // Check a string is a int ?
        private static bool isInt(string s)
        {
            int a;
            return int.TryParse(s, out a);
        }

        private static bool isInt(char c)
        {
            int a;
            return int.TryParse(c.ToString(), out a);
        }

        #endregion
    }
}
