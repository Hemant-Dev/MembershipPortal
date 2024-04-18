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

        public async Task<(IEnumerable<Discount>, int)> GetAllPaginatedDiscountAsync(int page, int pageSize, Discount discountObj)
        {
            var query = _dbContext.Discounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(discountObj.DiscountCode))
            {
                query = query.Where(discount => discount.DiscountCode.Contains(discountObj.DiscountCode));
            }
            if (discountObj.DiscountAmount >= 0)
            {
                query = query.Where(discount => discount.DiscountAmount == discountObj.DiscountAmount);
            }
            

            int totalCount = query.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalCount / pageSize));

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return (await query.ToListAsync(), totalPages);
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
