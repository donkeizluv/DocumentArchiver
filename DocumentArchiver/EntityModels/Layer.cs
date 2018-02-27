using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class Layer
    {
        public Layer()
        {
            User = new HashSet<User>();
        }

        public string LayerName { get; set; }
        public int Rank { get; set; }

        public ICollection<User> User { get; set; }
    }
}
