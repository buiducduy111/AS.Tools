using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Tools;
using AS.Tools.SeleniumHelper;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] test = new string[]
            {
                "duy bui 1995 duy dep Trai 12",
                "2158 duy xinh trai 281 duy",
                "duy 1995/06/11"
            };
            foreach (string s in test)
            {
                Console.WriteLine(StrHelper.IntDetect(s));
            }

            Server server = new Server("NWPUY4NMVPDGTE0U");
            string msg, param;
            bool a = server.HasUpdate("1.0");

            Console.ReadLine();
        }
    }
}
