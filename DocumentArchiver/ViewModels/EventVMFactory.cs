using DocumentArchiver.EntityModels;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class EventVMFactory
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private DocumentArchiverContext _context;

        public EventVMFactory(DocumentArchiverContext context)
        {
            _context = context;
        }

        public async Task<EventViewModel> Create(int id, int page, string orderBy, bool asc)
        {
            string checkedOrder = orderBy;
            bool checkedAsc = asc;
            //Default order
            if (string.IsNullOrEmpty(orderBy))
            {
                checkedOrder = nameof(Contract.CreateTime);
                checkedAsc = false;
            }

            var model = new EventViewModel
            {
                OnPage = page,
                OrderBy = checkedOrder,
                OrderAsc = checkedAsc
            };

            var query = GetEventsByContractId(id, page, model.ItemPerPage, checkedOrder, checkedAsc, out var totalRows);
            model.TotalRows = totalRows;
            model.Items = await query.ToListAsync();
            return model;

        }
        private IQueryable<EventLog> GetEventsByContractId(int id, int page, int take, string orderBy, bool asc, out int totalRows)
        {
            int excludedRows = (page - 1) * take;
            var query = _context.EventLog.Where(e => e.ContractId == id);
            totalRows = query.Count();
            var ordered = OrderTranslater(query, orderBy, asc);
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
