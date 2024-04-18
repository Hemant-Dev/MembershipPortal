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
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private readonly MembershipPortalDbContext _dbContext;

        public GenderRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Gender>> GetAllSortedGender(string? sortColumn, string? sortOrder)
        {
            IQueryable<Gender> query = _dbContext.Genders;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "gendername":
                        query = isAscending ? query.OrderBy(s => s.GenderName) : query.OrderByDescending(s => s.GenderName);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }
            }
            return await query.ToListAsync();
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
