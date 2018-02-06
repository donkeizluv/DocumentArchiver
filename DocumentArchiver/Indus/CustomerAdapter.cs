using System;
using System.Net;
using DocumentArchiver.EntityModels;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System.Linq;
using NLog;
using System.Data;
using System.Threading.Tasks;

namespace DocumentArchiver.Indus
{
    public class IndusAdapter : IIndusAdapter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private const string connectionStringTemplate =
            "User Id={username};Password={pwd};Data Source=(DESCRIPTION=" +
            "(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))" +
            "(CONNECT_DATA=(SID={sid})));";

        public int Port { get; private set; }
        public string Server { get; private set; }
        public string SID { get; private set; }
        public string Query { get; set; }
        private const string paramHolder = "{param}";

        public NetworkCredential Cred { get; private set; }
        public IndusAdapter(string server, int port, string sid, string userName, string pwd)
        {
            if (string.IsNullOrEmpty(server)) throw new ArgumentException();
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException();
            if (string.IsNullOrEmpty(sid)) throw new ArgumentException();
            if (string.IsNullOrEmpty(pwd)) throw new ArgumentException();

            Port = port;
            Server = server;
            SID = sid;
            Cred = new NetworkCredential(userName, pwd, "");
        }

        private string GetQuery(string contractId)
        {
            //Strip special chars
            return Query.Replace(paramHolder, contractId);
        }
        public string GetConnectionString()
        {
            return connectionStringTemplate.Replace("{username}", Cred.UserName)
                .Replace("{pwd}", Cred.Password)
                .Replace("{host}", Server)
                .Replace("{port}", Port.ToString())
                .Replace("{sid}", SID);
        }

        public async Task<Contract> GetContractInfoAsync(string contractId)
        {
            throw new NotImplementedException();
        }
        private Contract GetContract(string contractId)
        {
            using (var connection = new OracleConnection())
            {
                connection.ConnectionString = GetConnectionString();
                connection.OpenAsync();
                var cmd = new CommandDefinition(GetQuery(contractId));
                //Map to object
                var customer = connection.Query<Contract>(cmd);
                if (!customer.Any()) return null;
                var contract = customer.Single();
                //Fill other system value
                contract.CustomerName = contract.CustomerName.Normalize();
                contract.CreateTime = DateTime.Now;
                return contract;
            }
        }
        public Contract GetContractInfo(string contractId)
        {
            if (string.IsNullOrEmpty(Query))
                throw new InvalidOperationException("Query is not set");
            if (string.IsNullOrEmpty(contractId))
                throw new ArgumentNullException();
            return GetContract(contractId);
        }
    }
}
