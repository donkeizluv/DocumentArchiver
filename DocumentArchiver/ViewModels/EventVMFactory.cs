using DocumentArchiver.ApiParameter;
using DocumentArchiver.EntityModels;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class EventVMFactory : IViewModelFactory<EventViewModel>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private DocumentArchiverContext _context;

        public EventVMFactory(DocumentArchiverContext context)
        {
            _context = context;
        }

        public async Task<EventViewModel> Create(ListingParams apiParam)
        {
            string checkedOrder = apiParam.OrderBy;
            bool checkedAsc = apiParam.Asc;
            int checkedPage = apiParam.Page;
            //Default order
            if (string.IsNullOrEmpty(apiParam.OrderBy))
            {
                checkedOrder = nameof(Contract.CreateTime);
                checkedAsc = false;
            }
            if (checkedPage < 1) checkedPage = 1;

            var model = new EventViewModel
            {
                OnPage = checkedPage,
                OrderBy = checkedOrder,
                OrderAsc = checkedAsc
            };

            var query = GetEventsByContractId(apiParam, model.ItemPerPage, out var totalRows);
            model.TotalRows = totalRows;
            model.Items = await query.ToListAsync();
            return model;

        }
        private IQueryable<EventLog> GetEventsByContractId(ListingParams apiParam, int take, out int totalRows)
        {
            int excludedRows = (apiParam.Page - 1) * take;
            var query = _context.EventLog.Where(e => e.ContractId == apiParam.Id);
            totalRows = query.Count();
            var ordered = OrderTranslater(query, apiParam.OrderBy, apiParam.Asc);
            return ordered.Skip(excludedRows).Take(take);
        }

        private IOrderedQueryable<EventLog> OrderTranslater(IQueryable<EventLog> query, string order, bool asc)
        {
            var orderBy = order ?? string.Empty;
            switch (orderBy)
            {
                case nameof(EventLog.EventId):
                    if (!asc)
                        return query.OrderByDescending(r => r.EventId);
                    return query.OrderBy(r => r.EventId);
                case nameof(EventLog.Name):
                    if (!asc)
                        return query.OrderByDescending(r => r.Name);
                    return query.OrderBy(r => r.Name);
                case nameof(EventLog.DateOfEvent):
                    if (!asc)
                        return query.OrderByDescending(r => r.DateOfEvent);
                    return query.OrderBy(r => r.DateOfEvent);
                case nameof(EventLog.CreateTime):
                    if (!asc)
                        return query.OrderByDescending(r => r.CreateTime);
                    return query.OrderBy(r => r.CreateTime);
                case nameof(EventLog.Filetype):
                    if (!asc)
                        return query.OrderByDescending(r => r.Filetype);
                    return query.OrderBy(r => r.Filetype);
                default:
                    _logger.Debug($"Unknown <{nameof(orderBy)}> value: {orderBy}");
                    return query.OrderByDescending(r => r.EventId);
            }
        }
    }
}
