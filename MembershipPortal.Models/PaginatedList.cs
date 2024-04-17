using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Models
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool hasPrevious => CurrentPage > 1;
        public bool hasNext => CurrentPage < TotalPages;

        public PaginatedList()
        {
            
        }
    }
}
