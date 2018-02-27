using DocumentArchiver.ApiParameter;
using DocumentArchiver.EntityModels;
using DocumentArchiver.Logic;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class ContractVMFactory : IViewModelFactory<ContractViewModel>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private DocumentArchiverContext _context;
        public ContractVMFactory(DocumentArchiverContext context)
        {
            _context = context;
        }

        public async Task<ContractViewModel> Create(ListingParams apiParam)
        {
            string checkedOrder = apiParam.OrderBy;
            bool checkedAsc = apiParam.Asc;
            int checkedPage = apiParam.Page;
            //Default order
            if(string.IsNullOrEmpty(apiParam.OrderBy))
            {
                checkedOrder = nameof(Contract.CreateTime);
                checkedAsc = false;
            }
            if (checkedPage < 1) checkedPage = 1;

            var model = new ContractViewModel
            {
                OnPage = checkedPage,
                FilterBy = apiParam.Type,
                FilterString = apiParam.Contain,
                OrderBy = checkedOrder,
                OrderAsc = checkedAsc
            };

            var query = GetFilteredContracts(apiParam, model.ItemPerPage, out var totalRows);
            model.TotalRows = totalRows;
            model.Items = await query.ToListAsync();
            return model;

        }
        private IQueryable<Contract> GetFilteredContracts(ListingParams apiParam, int take, out int totalRows)
        {
            int excludedRows = (apiParam.Page - 1) * take;
            //Apply logic/what to show based on layer here
            var baseQuery = ContractLayer.GetContractSet(apiParam.HttpContext, apiParam.DbContext);
            //var baseQuery = _context.Contract.AsQueryable();
            totalRows = baseQuery.Count();
            //Check if filter is applied
            if (!string.IsNullOrEmpty(apiParam.Type) && !string.IsNullOrEmpty(apiParam.Contain))
            {
                baseQuery = FilterTranslater(baseQuery, apiParam.Type, apiParam.Contain);
                totalRows = baseQuery.Count();
            }
            var ordered = OrderTranslater(baseQuery, apiParam.OrderBy, apiParam.Asc);
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
