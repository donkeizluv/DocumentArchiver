using Newtonsoft.Json;
using System;

namespace DocumentArchiver.EntityModels
{
    public partial class EventLog
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public DateTime DateOfEvent { get; set; }
        [JsonIgnore]
        public DateTime CreateTime { get; set; }
        public string Note { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public string Guid { get; set; }
        public string Filetype { get; set; }
        [JsonIgnore]
        public int ContractId { get; set; }
        public string Username { get; set; }

        public Contract Contract { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
