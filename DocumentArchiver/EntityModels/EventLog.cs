using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class EventLog
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEvent { get; set; }
        public DateTime CreateTime { get; set; }
        public string Note { get; set; }
        public string Filename { get; set; }
        public string Filetype { get; set; }
        public int ContractId { get; set; }
        public string Username { get; set; }

        public Contract Contract { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
