using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
    public interface IGenderRepository : IRepository<Gender>
    {
        Task<IEnumerable<Gender>> SearchAsyncAll(string search);
        Task<(IEnumerable<Gender>, int)> GetAllPaginatedGenderAsync(int page, int pageSize, Gender gender);
    }
}

