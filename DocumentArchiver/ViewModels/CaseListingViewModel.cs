using DocumentArchiver.EntityModels;
using System.Collections.Generic;

namespace DocumentArchiver.ViewModels
{
    public class CaseListingViewModel
    {
        public List<Contract> Contracts { get; set; }
        public List<string> DocumentNames { get; set; }
        //For debug purposes
        public int OnPage { get; set; }
        public string FilterBy { get; set; }
        public string FilterString { get; set; }
        public string OrderBy { get; set; }
        public bool OrderAsc { get; set; }
        public static int ItemPerPage { get; set; } = 10;

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