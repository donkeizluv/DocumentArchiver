using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    public static class Const
    {
        public static string StandardDate = "dd/MM/yyyy";
        public static string OnlyNumberDate = "ddMMyyyy";
        public static string JavascriptDate = "yyyy-MM-dd";
        public static string[] AcceptedEventFile = { ".jpg", ".jpeg", ".bmp", ".png", ".doc", ".docx", ".pdf", ".msg" };
    }
}
