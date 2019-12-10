using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Inputs
{
    public class RequestPaginatorWithUser : RequestPaginatorWithSearch
    {
        public int IdUser { get; set; }
        public string Tenant { get; set; }
        public int StatusId { get; set; }
    }
}