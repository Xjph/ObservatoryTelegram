using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Observatory.Telegram
{
    public static class Logger
    {
        private static bool headerSent = false;

        public static void AppendLog(string theMessage,string theFile,string theVersion)
        {
            string ret = "";
            if (!headerSent)
            {
                ret += $"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.s")} {theVersion}\n";
                headerSent = true;
            }
            ret += $"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.s")} {theMessage}\n";
            try
            {
                File.AppendAllText(theFile, ret);
            }
            catch { }

        }

    }
}
