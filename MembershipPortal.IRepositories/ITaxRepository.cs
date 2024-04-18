using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{

    public interface ITaxRepository : IRepository<Tax>
    {
        Task<IEnumerable<Tax>> GetAllSortedTax(string? sortColumn, string? sortOrder);
    }

}
