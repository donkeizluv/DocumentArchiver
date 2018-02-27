using DocumentArchiver.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.EntityModels
{
    public partial class EventLog
    {
        public string DateOfEventString
        {
            get
            {
                return DateOfEvent.ToString(AppConst.StandardDate);
            }
        }
        public string DateOfEventJS
        {
            get
            {
                return DateOfEvent.ToString(AppConst.JavascriptDate);
            }
        }
        public string CreateTimeString
        {
            get
            {
                return CreateTime.ToString(AppConst.StandardDate);
            }
        }
    }
}
