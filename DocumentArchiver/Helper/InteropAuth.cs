using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    public class InteropAuth
    {
        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);
    }
}
