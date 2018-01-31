using DocumentArchiver.EntityModels;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class ContractVMFactory
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private DocumentArchiverContext _context;
        public ContractVMFactory(DocumentArchiverContext context)
        {
            _context = context;
        }

        public async Task<ContractViewModel> Create(int page, string type, string contains, string orderBy, bool asc)
        {
            string checkedOrder = orderBy;
            bool checkedAsc = asc;
            //Default order
            if(string.IsNullOrEmpty(orderBy))
            {
                checkedOrder = nameof(Contract.CreateTime);
                checkedAsc = false;
            }
            
            var model = new ContractViewModel
            {
                OnPage = page,
                FilterBy = type,
                FilterString = contains,
                OrderBy = checkedOrder,
                OrderAsc = checkedAsc
            };

            var query = GetFilteredContracts(page, model.ItemPerPage, type, contains, checkedOrder, checkedAsc, out var totalRows);
            model.TotalRows = totalRows;
            model.Items = await query.ToListAsync();
            return model;

        }
        private IQueryable<Contract> GetFilteredContracts(int page, int take, string type, string contains, string orderBy, bool asc, out int totalRows)
        {
            int excludedRows = (page - 1) * take;
            var query = _context.Contract.AsQueryable();
            totalRows = query.Count();
            //Check if filter is applied
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(contains))
            {
                query = FilterTranslater(query, type, contains);
                totalRows = query.Count();
            }
            var ordered = OrderTranslater(query, orderBy, asc);
            return ordered.Skip(excludedRows).Take(take);
        }

        private IOrderedQueryable<Contract> OrderTranslater(IQueryable<Contract> query, string order, bool asc)
        {
            var orderBy = order ?? string.Empty;
            switch (orderBy)
            {
                case nameof(Contract.ContractNumber):
                    if (!asc)
                        return query.OrderByDescending(r => r.ContractNumber);
                    return query.OrderBy(r => r.ContractNumber);
                case nameof(Contract.ContractId):
                    if (!asc)
                        return query.OrderByDescending(r => r.ContractId);
                    return query.OrderBy(r => r.ContractId);
                case nameof(Contract.CustomerName):
                    if (!asc)
                        return query.OrderByDescending(r => r.CustomerName);
                    return query.OrderBy(r => r.CustomerName);
                case nameof(Contract.IdentityCard):
                    if (!asc)
                        return query.OrderByDescending(r => r.IdentityCard);
                    return query.OrderBy(r => r.IdentityCard);
                case nameof(Contract.Phone):
                    if (!asc)
                        return query.OrderByDescending(r => r.Phone);
                    return query.OrderBy(r => r.Phone);
                case nameof(Contract.CreateTime):
                    if (!asc)
                        return query.OrderByDescending(r => r.CreateTime);
                    return query.OrderBy(r => r.CreateTime);
                case nameof(Contract.Username):
                    if (!asc)
                        return query.OrderByDescending(r => r.Username);
                    return query.OrderBy(r => r.Username);
                case "":
                    return query.OrderByDescending(r => r.ContractId);
                default:
                    _logger.Debug($"Unknown <{nameof(orderBy)}> value: {orderBy}");
                    return query.OrderByDescending(r => r.ContractId);
            }
        }

        private IQueryable<Contract> FilterTranslater(IQueryable<Contract> query, string filterBy, string filterText)
        {
            switch (filterBy)
            {
                case nameof(Contract.ContractNumber):
                    return query.Where(c => c.ContractNumber.Contains(filterText));
                case nameof(Contract.CustomerName):
                    return query.Where(c => c.CustomerName.Contains(filterText));
                case nameof(Contract.IdentityCard):
                    return query.Where(c => c.IdentityCard.Contains(filterText));
                case nameof(Contract.Phone):
                    return query.Where(c => c.Phone.Contains(filterText));
                case nameof(Contract.Username):
                    return query.Where(c => c.Username.Contains(filterText));
                default:
                    _logger.Debug($"Unknown <{nameof(filterBy)}> value: {filterBy}");
                    return query.Where(c => c.CustomerName.Contains(filterText));
            }
        }
    }
}
