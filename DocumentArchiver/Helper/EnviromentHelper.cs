using Microsoft.AspNetCore.Http;
using NLog;
using System;

namespace DocumentArchiver.Helper
{
    public static class EnviromentHelper
    {
        public static string EnvStr = string.Empty;
        public static string ConnectionStringKey = "DbConnectionString";
       
        public static PathString LoginUrl
        {
            get
            {
                return new PathString("/Account/Login");
            }
        }

    }
}
