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
    public class TaxRepository : Repository<Tax>, ITaxRepository
    {
        private readonly MembershipPortalDbContext _dbContext;
        public TaxRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Tax>> GetAllSortedTax(string? sortColumn, string? sortOrder)
        {
            IQueryable<Tax> query = _dbContext.Taxes;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "taxname":
                        query = isAscending ? query.OrderBy(s => s.TaxName) : query.OrderByDescending(s => s.TaxName);
                        break;
                    case "cgst":
                        query = isAscending ? query.OrderBy(s => s.CGST) : query.OrderByDescending(s => s.CGST);
                        break;
                    case "sgst":
                        query = isAscending ? query.OrderBy(s => s.SGST) : query.OrderByDescending(s => s.SGST);
                        break;
                    case "totaltax":
                        query = isAscending ? query.OrderBy(s => s.TotalTax) : query.OrderByDescending(s => s.TotalTax);
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
