using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace web_api.Utils
{
    public class Logger
    {
        public string Ex { get; set; }
    
    public Logger(string logPath, Exception ex) 
        {
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.Write("Data:");
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Write("Mensagem:");
                sw.WriteLine(ex.Message);
                sw.Write("StackTrace:");
                sw.WriteLine(ex.StackTrace);
            }


        }



    }
}