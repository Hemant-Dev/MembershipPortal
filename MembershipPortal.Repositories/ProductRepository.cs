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
    public class ProductRepository : Repository<Product>, IProductRepository
    {


        private readonly MembershipPortalDbContext _dbContext;
        public ProductRepository(MembershipPortalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Product>> GetProductSearchAsync(string find)
        {
            string keyword = find.ToLower();

            var filterlist = await _dbContext.Products
               .Where(m => m.ProductName.ToLower().Contains(keyword) ||
                m.Price.ToString().Contains(keyword))
                .ToListAsync();

            return filterlist;
        }


        public async Task<IEnumerable<Product>> GetProductAdvanceSearchAsync(Product productobj)
        {
            var query = _dbContext.Products.AsQueryable();

        

            if (!string.IsNullOrWhiteSpace(productobj.ProductName))
            {
                query = query.Where(product => productobj.ProductName == product.ProductName);
            }
            if (productobj.Price > 0)
            {
                query = query.Where(product => product.Price == productobj.Price);
            }

            return await query.ToListAsync();

        }

        public async Task<(IEnumerable<Product>, int)> GetAllPaginatedProductAsync(int page, int pageSize)
        {
            var productsList = await _dbContext.Products.ToListAsync();
            int totalCount = productsList.Count;
            int totalPages = (int)(Math.Ceiling((decimal) totalCount / pageSize));
            productsList = productsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (productsList, totalPages);
        }

        public async Task<IEnumerable<Product>> GetAllSortedProducts(string? sortColumn, string? sortOrder)
        {
            IQueryable<Product> query = _dbContext.Products;
            if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                // Determine the sort order based on sortOrder parameter
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortColumn.ToLower())
                {
                    case "productname":
                        query = isAscending ? query.OrderBy(s => s.ProductName) : query.OrderByDescending(s => s.ProductName);
                        break;
                    case "price":
                        query = isAscending ? query.OrderBy(s => s.Price) : query.OrderByDescending(s => s.Price);
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
