using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc
{
    public static class AppSettings
    {
        private static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
       
        public static string Environment => Get("Environment");

        public static bool IsLocal => Environment == "localhost";

    }
}