using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetAllSortedDiscount(string? sortColumn, string? sortOrder);
    }
}
