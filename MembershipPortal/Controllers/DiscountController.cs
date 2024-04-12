using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.API.ErrorHandling;

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        public string tableName = "Discount";

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: api/Discount
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDiscountDTO>>> GetDiscountsAsyc()
        {
            try
            {
                var discoutDTOList = await _discountService.GetDiscountsAsync();
                if (discoutDTOList != null)
                {
                    return Ok(discoutDTOList);
                }
                return NotFound(MyException.DataNotFound(tableName));
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // GET: api/Discount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDiscountDTO>> GetDiscountByIdAsync(long id)
        
        {
            try
            {
                var discountDTO = await _discountService.GetDiscountByIdAsync(id);

                if (discountDTO == null)
                {
                    return NotFound(id);
                }

                return NotFound(MyException.DataWithIdNotPresent(id,tableName));
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
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
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }

                var result =  await _discountService.UpdateDiscountAsync(id, discountDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
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
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // DELETE: api/Discount/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDiscountAsync(long id)
        {
            try
            {
                var discountDTO = await _discountService.GetDiscountByIdAsync(id);
                if (discountDTO != null)
                {
                    var result = await _discountService.DeleteDiscountAsync(id);
                    return Ok(MyException.DataDeletedSuccessfully(tableName));
                }
                return NotFound(MyException.DataWithIdNotPresent(id, tableName));
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

    }
}
