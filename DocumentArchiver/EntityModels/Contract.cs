using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class Contract
    {
        public Contract()
        {
            ContractSharing = new HashSet<ContractSharing>();
            EventLog = new HashSet<EventLog>();
        }

        public int ContractId { get; set; }
        public string CustomerName { get; set; }
        public string IdentityCard { get; set; }
        public string Phone { get; set; }
        public DateTime CreateTime { get; set; }
        public string Username { get; set; }

        public User UsernameNavigation { get; set; }
        public ICollection<ContractSharing> ContractSharing { get; set; }
        public ICollection<EventLog> EventLog { get; set; }
    }
}
