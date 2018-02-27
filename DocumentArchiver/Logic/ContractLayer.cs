using DocumentArchiver.EntityModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DocumentArchiver.Logic
{
    static public class ContractLayer
    {
        public static IQueryable<Contract> GetContractSet(HttpContext httpContext, DocumentArchiverContext dbContext)
        {
            if (httpContext == null) throw new ArgumentNullException();
            if(dbContext == null) throw new ArgumentNullException();
            if (!TryGetLayerData(httpContext, out var rank))
                return dbContext.Contract.Where(c => false); //Return empty if no layer found
            return dbContext.Contract.Where(c => c.UsernameNavigation.LayerNameNavigation.Rank <= rank);
        }
        private static bool TryGetLayerData(HttpContext context, out int rank)
        {
            rank = -1;
            var claim = context.User.Claims.FirstOrDefault(i => i.Type == Helper.AppConst.LayerRank);
            if (claim == null) return false;
            if (!int.TryParse(claim.Value, out rank)) return false;
            return true;
        }
    }
}
