using MembershipPortal.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MembershipPortal.DTOs.ProductDTO;

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
    }
}
