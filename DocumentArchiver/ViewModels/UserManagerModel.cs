using DocumentArchiver.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public class UserManagerModel : ViewModel<User>
    {
        public override int ItemPerPage => 10;
    }
}
