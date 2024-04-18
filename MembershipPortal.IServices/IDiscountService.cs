using MembershipPortal.DTOs;
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
    public interface IDiscountService
    {
        Task<IEnumerable<GetDiscountDTO>> GetDiscountsAsync();
        Task<GetDiscountDTO> GetDiscountByIdAsync(long id);
        Task<GetDiscountDTO> CreateDiscountAsync(CreateDiscountDTO discountDTO);
        Task<GetDiscountDTO> UpdateDiscountAsync(long Id, UpdateDiscountDTO discoutDTO);
        Task<bool> DeleteDiscountAsync(long Id);

        Task<IEnumerable<GetDiscountDTO>> GetAllSortedDiscounts(string? sortColumn, string? sortOrder);
        public Task<(IEnumerable<GetDiscountDTO>, int)> GetAllPaginatedDiscountAsync(int page, int pageSize, Discount discount);
    }
}
