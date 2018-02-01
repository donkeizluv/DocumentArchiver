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
                return DateOfEvent.ToString(Const.StandardDate);
            }
        }
        public string DateOfEventJS
        {
            get
            {
                return DateOfEvent.ToString(Const.JavascriptDate);
            }
        }
        public string CreateTimeString
        {
            get
            {
                return CreateTime.ToString(Const.StandardDate);
            }
        }
    }
}
