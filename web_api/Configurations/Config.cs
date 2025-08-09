using System;
using System.IO;

namespace web_api.Configurations
{
    public static class Config
    {
        public static string GetLogPath()
        {
            return GetLogPath("logPath");
        }

        public static string GetLogPath(string chave)
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings[chave];
            
            logPath = Path.Combine(logPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
            
            return logPath ;
        }

        public static string GetConnectionStringSQLServer()
        {
            return GetConnectionStringSQLServer("web_api");
        }

        public static string GetConnectionStringSQLServer(string nome)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[nome].ConnectionString;
        }

        public static int GetCacheExpirationTimeInSeconds(string chave) 
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[chave]);
        }
    }
}