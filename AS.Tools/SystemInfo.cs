using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// System info
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// Get CPU ID
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }
            return id;
        }
    }
}
