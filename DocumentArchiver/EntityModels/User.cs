using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class User
    {
        public User()
        {
            Contract = new HashSet<Contract>();
            EventLog = new HashSet<EventLog>();
            UserAbility = new HashSet<UserAbility>();
        }

        public string Username { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLogin { get; set; }

        public ICollection<Contract> Contract { get; set; }
        public ICollection<EventLog> EventLog { get; set; }
        public ICollection<UserAbility> UserAbility { get; set; }
    }
}
