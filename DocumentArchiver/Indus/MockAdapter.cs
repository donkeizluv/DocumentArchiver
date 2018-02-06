using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentArchiver.EntityModels;

namespace DocumentArchiver.Indus
{
    public class MockAdapter : IIndusAdapter
    {
        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public Contract GetContractInfo(string contractNumber)
        {
            return new Contract()
            {
                ContractNumber = contractNumber,
                CreateTime = DateTime.Now,
                CustomerName = "hong",
                IdentityCard = "12345",
                Phone = "54321"
            };
        }

        public Task<Contract> GetContractInfoAsync(string contractId)
        {
            throw new NotImplementedException();
        }
    }
}
