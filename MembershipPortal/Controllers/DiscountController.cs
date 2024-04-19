using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.API.ErrorHandling;
using MembershipPortal.Services;
using static MembershipPortal.DTOs.ProductDTO;
using MembershipPortal.Models;
using static MembershipPortal.DTOs.UserDTO;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: api/Discount
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<GetDiscountDTO>>> GetDiscountsAsyc()
        //{
        //    try
        //    {
        //        var discoutDTOList = await _discountService.GetDiscountsAsync();
        //        if (discoutDTOList != null)
        //        {
        //            return Ok(discoutDTOList);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public async Task<ActionResult<IEnumerable<GetDiscountDTO>>> Get(string? sortColumn, string? sortOrder)
        {
            try
            {
                var getDiscountsDTOList = await _discountService.GetAllSortedDiscounts(sortColumn, sortOrder);
                if (getDiscountsDTOList != null)
                {
                    return Ok(getDiscountsDTOList);
                }
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/Discount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDiscountDTO>> GetDiscountByIdAsync(long id)
        {
            try
            {
                var discountDTO = await _discountService.GetDiscountByIdAsync(id);

                if (discountDTO != null)
                {
                    return discountDTO;
                }

                return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT: api/Discount/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<GetDiscountDTO>> PutDiscountAsync(long id, UpdateDiscountDTO discountDTO)
        {
            if (id != discountDTO.Id)
            {
                return BadRequest(MyException.IdMismatch());
            }
            
            try
            {
                var oldDiscount = await _discountService.GetDiscountByIdAsync(id);

                if (oldDiscount == null)
                {
                    return NotFound();
                }

                var result =  await _discountService.UpdateDiscountAsync(id, discountDTO);

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        // POST: api/Discount
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetDiscountDTO>> PostDiscountAsync(CreateDiscountDTO discountDTO)
        {
            try
            {
                var result = await _discountService.CreateDiscountAsync(discountDTO);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Discount/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDiscountAsync(long id)
        {
            try
            {
                var result = await _discountService.DeleteDiscountAsync(id);
                return result;   
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("paginated")]
        public async Task<ActionResult<Paginated<GetDiscountDTO>>> GetPaginatedUserData(string? sortColumn, string? sortOrder, int page, int pageSize, [FromBody] GetDiscountDTO discount)
        {
            try
            {
                var paginatedDiscountDTOAndTotalPages = await _discountService.GetAllPaginatedDiscountAsync(page, pageSize, new Discount()
                {
                    DiscountCode = discount.DiscountCode,
                    DiscountAmount = discount.DiscountAmount,
                    IsDiscountInPercentage = discount.IsDiscountInPercentage,
                });
                var result = new Paginated<GetDiscountDTO>
                {
                    dataArray = paginatedDiscountDTOAndTotalPages.Item1,
                    totalPages = paginatedDiscountDTOAndTotalPages.Item2
                };
                if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
                {
                    // Determine the sort order based on sortOrder parameter
                    bool isAscending = sortOrder.ToLower() == "asc";
                    switch (sortColumn.ToLower())
                    {
                        case "discountcode":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.DiscountCode) : result.dataArray.OrderByDescending(s => s.DiscountCode);
                            break;
                        case "discountamount":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.DiscountAmount) : result.dataArray.OrderByDescending(s => s.DiscountAmount);
                            break;
                        default:
                            result.dataArray = result.dataArray.OrderBy(s => s.Id);
                            break;
                    }

                }
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
