using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
   
    public interface IProductRepository : IRepository<Product>
    {

        Task<IEnumerable<Product>> GetProductSearchAsync(String find);

        Task<IEnumerable<Product>> GetProductAdvanceSearchAsync(Product productobj);

        Task<(IEnumerable<Product>, int)> GetAllPaginatedProductAsync(int page, int pageSize, Product product);

        Task<IEnumerable<Product>> GetAllSortedProducts(string? sortColumn, string? sortOrder);
        
    }
}
