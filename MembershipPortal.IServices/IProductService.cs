using MembershipPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.ProductDTO;
using static MembershipPortal.DTOs.UserDTO;

namespace MembershipPortal.IServices
{
    public interface IProductService
    {

        public Task<IEnumerable<GetProductDTO>> GetProductsAsync();
        public Task<GetProductDTO> GetProductAsync(long Id);

        public Task<GetProductDTO> CreateProductAsync(CreateProductDTO createProductDTO);

        public Task<GetProductDTO> UpdateProductAsync(long Id, UpdateProductDTO updateProductDTO);

        public Task<bool> DeleteProductAsync(long Id);

        public Task<IEnumerable<GetProductDTO>> GetProductSearchAsync(string find);

        public Task<IEnumerable<GetProductDTO>> GetProductAdvanceSearchAsync(GetProductDTO getProductDTO);

        public Task<(IEnumerable<GetProductDTO>, int)> GetAllPaginatedProductAsync(int page, int pageSize, Product product);

        public Task<IEnumerable<GetProductDTO>> GetAllSortedProducts(string? sortColumn, string? sortOrder);
    }
}
