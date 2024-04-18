using MembershipPortal.Data;
using MembershipPortal.IRepositories;
using MembershipPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly MembershipPortalDbContext _dbContext;
        public DiscountRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Discount>> GetAllSortedDiscount(string? sortColumn, string? sortOrder)
        {
            IQueryable<Discount> query = _dbContext.Discounts;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "discountcode":
                        query = isAscending ? query.OrderBy(s => s.DiscountCode) : query.OrderByDescending(s => s.DiscountCode);
                        break;
                    case "discountamount":
                        query = isAscending ? query.OrderBy(s => s.DiscountAmount) : query.OrderByDescending(s => s.DiscountAmount);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }

            }

            return await query.ToListAsync();
        }
    }
}
