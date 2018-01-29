using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class UserAbility
    {
        public string AbilityName { get; set; }
        public string Username { get; set; }

        public Ability AbilityNameNavigation { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
