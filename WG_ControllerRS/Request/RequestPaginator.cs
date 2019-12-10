using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Inputs
{
    public class RequestPaginator
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }

    public class RequestPaginatorWithSearch : RequestPaginator
    {
        public string Filters { get; set; }
    }
}
