﻿using DocumentArchiver.EntityModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    //session related stuff goes here
    public static class SessionHelper
    {
        public static Dictionary<string, List<string>> ClaimsToDict(IEnumerable<Claim> claims)
        {
            var dict = new Dictionary<string, List<string>>();
            foreach (var claim in claims)
            {
                if (dict.ContainsKey(claim.Type))
                    dict[claim.Type].Add(claim.Value);
                else
                    dict.Add(claim.Type, new List<string>() { claim.Value });
            }
            return dict;
        }
    }
}
