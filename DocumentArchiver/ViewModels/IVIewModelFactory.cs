﻿using DocumentArchiver.ApiParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    interface IViewModelFactory<T> where T : IViewModel
    {
        Task<T> Create(ListingParams apiParam);
    }
}
