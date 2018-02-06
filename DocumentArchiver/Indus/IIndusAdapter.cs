using DocumentArchiver.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.Indus
{
    public interface IIndusAdapter
    {
        string GetConnectionString();
        Task<Contract> GetContractInfoAsync(string contractId);
        Contract GetContractInfo(string contractId);
    }
}
