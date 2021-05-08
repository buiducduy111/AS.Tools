using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// Log helper
    /// </summary>
    public class Logger
    {
        private string _logFile;
        private List<string> _logData { get; set; }

        /// <summary>
        /// Init with set log file name
        /// </summary>
        /// <param name="logFile"></param>
        public Logger(string logFile)
        {
            _logFile = logFile;
            init();
        }

        /// <summary>
        /// Init with default log file name: ddMMyyyy
        /// </summary>
        public Logger()
        {
            _logFile = $"log_{DateTime.Now.ToString("ddMMyyyy")}";
            init();
        }

        /// <summary>
        /// Apend log to log data
        /// </summary>
        /// <param name="text"></param>
        public void Append(string text)
        {
            _logData.Add($"[{DateTime.Now.ToString("hh:mm:ss")}] : {text}");
        }

        /// <summary>
        /// Clear all log data
        /// </summary>
        public void Clear()
        {
            _logData.Clear();
        }

        /// <summary>
        /// Save to log\_logFile
        /// </summary>
        public void Save()
        {
            File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + $"\\log\\{_logFile}.txt", _logData);
            _logData.Clear();
        }

        private void init()
        {
            if (!Directory.Exists("log"))
                Directory.CreateDirectory("log");

            _logData = new List<string>();
            _logData.Add($"------------- NEW LOG START AT {DateTime.Now.ToString("hh:mm:ss")} -------------");
        }
    }
}
