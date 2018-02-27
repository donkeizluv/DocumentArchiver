using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class ActionLog
    {
        public int LogId { get; set; }
        public DateTime Created { get; set; }
        public string Action { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Ref { get; set; }
    }
}
