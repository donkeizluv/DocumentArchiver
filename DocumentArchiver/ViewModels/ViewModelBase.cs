using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentArchiver.ViewModels
{
    public abstract class ViewModelBase<T>
    {
        public List<T> Items { get; set; }
        //For debug purposes
        public virtual int OnPage { get; set; }
        public virtual string FilterBy { get; set; }
        public virtual string FilterString { get; set; }
        public virtual string OrderBy { get; set; }
        public virtual bool OrderAsc { get; set; }
        public abstract int ItemPerPage { get; }

        //update these every time add record
        public int TotalPages { get; private set; }
        private int _totalRows;
        public int TotalRows
        {
            get
            {
                return _totalRows;
            }
            set
            {
                _totalRows = value;
                TotalPages = (_totalRows + ItemPerPage - 1) / ItemPerPage;
                if (TotalPages < 1)
                    TotalPages = 1;
            }
        }
    }
}
