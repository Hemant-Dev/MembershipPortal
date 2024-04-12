using MembershipPortal.Data;
using MembershipPortal.Models;
using MembershipPortal.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NuGet.Common.NuGetEventSource;

namespace MembershipPortal.IRepositories
{
    public class GenderRepository :  Repository<Gender>, IGenderRepository
    {
        private readonly MembershipPortalDbContext _dbContext;

        public GenderRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Gender>> SearchAsyncAll(string search)
        {
            var keyword = search.ToLower();

            var filterlist = await _dbContext.Genders
                            .Where(gender => gender.GenderName.ToLower().Contains(keyword)).ToListAsync();

            return filterlist;
        }
    }
}
