﻿using DocumentArchiver.EntityModels;
using System.Collections.Generic;

namespace DocumentArchiver.ViewModels
{
    public class ContractViewModel : ViewModel<Contract>
    {
        public override int ItemPerPage => 10;
    }
}