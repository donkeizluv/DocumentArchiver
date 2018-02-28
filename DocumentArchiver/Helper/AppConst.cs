using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    public static class AppConst
    {
        public static readonly string StandardDate = "dd/MM/yyyy";
        public static readonly string OnlyNumberDate = "ddMMyyyy";
        public static readonly string JavascriptDate = "yyyy-MM-dd";
        public static readonly string NA = "N/A";
        public static readonly string JSONNull = "null";
        public static readonly string[] AcceptedEventFile = { ".jpg", ".jpeg", ".bmp", ".png", ".doc", ".docx", ".pdf", ".msg" };

        //Custom claim types
        public static readonly string UserName = "Username";
        public static readonly string Ability = "Ability";
        public static readonly string LayerName = "LayerName";
        public static readonly string LayerRank = "LayerRank";
    }
}
