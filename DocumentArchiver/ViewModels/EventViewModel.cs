﻿using DocumentArchiver.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class EventViewModel : ViewModel<EventLog>
    {
        public override int ItemPerPage => 5;
    }
}
