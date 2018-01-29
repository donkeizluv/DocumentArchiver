using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class Ability
    {
        public Ability()
        {
            UserAbility = new HashSet<UserAbility>();
        }

        public string AbilityName { get; set; }
        public string Description { get; set; }

        public ICollection<UserAbility> UserAbility { get; set; }
    }
}
