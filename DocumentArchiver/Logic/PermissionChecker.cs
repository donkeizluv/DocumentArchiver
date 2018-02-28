using DocumentArchiver.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentArchiver.Logic
{
    public static class PermissionChecker
    {
        public static bool CanCreate(IEnumerable<Claim> claims)
        {
            return HasAbility(claims, AbilityList.Create);
        }
        public static bool CanDelete(IEnumerable<Claim> claims)
        {
            return HasAbility(claims, AbilityList.Delete);
        }
        public static bool CanUpdate(IEnumerable<Claim> claims)
        {
            return HasAbility(claims, AbilityList.Update);
        }
        public static bool CanDownload(IEnumerable<Claim> claims)
        {
            return HasAbility(claims, AbilityList.Download);
        }
        public static bool CanManageUser(IEnumerable<Claim> claims)
        {
            return HasAbility(claims, AbilityList.ManageUser);
        }
        private static bool HasAbility(IEnumerable<Claim> claims, string name)
        {
            if (claims == null) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(name)) return false;

            foreach (var claim in claims.Where(c => c.Type == AppConst.Ability))
            {
                if (claim.Value == name)
                    return true;
            }
            return false;
        }
    }
}
