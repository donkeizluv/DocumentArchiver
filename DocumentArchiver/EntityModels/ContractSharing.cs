using System;
using System.Collections.Generic;

namespace DocumentArchiver.EntityModels
{
    public partial class ContractSharing
    {
        public int ShareId { get; set; }
        public string Keycode { get; set; }
        public bool Disabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DisabledOn { get; set; }
        public int Hit { get; set; }
        public int ContractId { get; set; }

        public Contract Contract { get; set; }
    }
}
