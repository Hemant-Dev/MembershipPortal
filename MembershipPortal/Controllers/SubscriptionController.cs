using MembershipPortal.API.ErrorHandling;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using MembershipPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static MembershipPortal.DTOs.UserDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly string tableName = "Subscription";
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }


        // GET: api/<SubscriptionController>
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> Get()
        //{
        //    try
        //    {
        //        var getSubscriptionDTOList = await _subscriptionService.GetAllSubscriptionForeignAsync();
        //        if(getSubscriptionDTOList != null)
        //        {
        //            return Ok(getSubscriptionDTOList);
        //        }
        //        return NotFound()
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> Get(string? sortColumn, string? sortOrder)
        {
            try
            {
                var getSubscriptionDTOList = await _subscriptionService.GetAllSortedSubscriptions(sortColumn, sortOrder);
                if (getSubscriptionDTOList != null)
                {
                    return Ok(getSubscriptionDTOList);
                }
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET api/<SubscriptionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSubscriptionDTO>> Get(long id)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionByIdAsync(id);
                if(result != null) {
                    return Ok(result);
                }
                return NotFound(MyException.DataWithIdNotPresent(id, tableName));
            }
            catch (Exception)
            {
                throw;
                //return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // POST api/<SubscriptionController>
        [HttpPost]
        public async Task<ActionResult<GetSubscriptionDTO>> Post([FromBody] CreateSubscriptionDTO subscriptionDTO)
        {
            try
            {
                var result = await _subscriptionService.CreateSubscriptionAsync(subscriptionDTO);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("Failed To create entry to the database table");
            }
            catch (Exception)
            {
                throw;
                //return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // PUT api/<SubscriptionController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GetSubscriptionDTO>> Put(long id, [FromBody] UpdateSubscriptionDTO updateSubscriptionDTO)
        {
            try
            {
                var result = await _subscriptionService.UpdateSubscriptionAsync(id, updateSubscriptionDTO);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }

            catch (Exception)
            {
                throw;
                //return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }

        }

        // DELETE api/<SubscriptionController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionByIdAsync(id);
                if (result != null)
                {
                    var subscription = await _subscriptionService.DeleteSubscriptionByIdAsync(id);
                    return subscription;
                }
                return NotFound();


            }
            catch (Exception)
            {
                throw;
                //return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }

        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> SubscriptionSearchAsync(string filter)
        {
            try
            {
                var subscriptionInfo = await _subscriptionService.GetAllSubscriptionSearchAsync(filter);
                return Ok(subscriptionInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving Subscription info : {ex.Message}");

            }

        }

        [HttpPost("advancesearch")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> SubscriptionAdvanceSearchAsync(GetSubscriptionDTO subscriptionDTO)
        {
            try
            {
                var filterData = await _subscriptionService.GetAllSubscriptionAdvanceSearchAsync(subscriptionDTO);
                return Ok(filterData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving Advance search Subscription info : {ex.Message}");

            }
        }

        [HttpPost("paginated")]
        public async Task<ActionResult<Paginated<GetSubscriptionDTO>>> GetPaginatedUserData(string? sortColumn, string? sortOrder, int page, int pageSize, [FromBody] GetSubscriptionDTO subscription)
        {
            try
            {
                var paginatedSubscriptionDTOAndTotalPages = await _subscriptionService.GetAllPaginatedSubscriptionAsync(sortColumn, sortOrder, page, pageSize, new Subscription()
                {
                    SubscriberId = subscription.SubscriberId,
                    ProductId = subscription.ProductId,
                    ProductName = subscription.ProductName,
                    ProductPrice = subscription.ProductPrice,
                    DiscountId = subscription.DiscountId,
                    DiscountCode = subscription.DiscountCode,
                    DiscountAmount = subscription.DiscountAmount,
                    TaxId = subscription.TaxId,
                    CGST = subscription.CGST,
                    SGST = subscription.SGST,
                    TotalTaxPercentage = subscription.TotalTaxPercentage,
                    StartDate = String.IsNullOrEmpty(subscription.StartDate.ToString()) ? DateOnly.MinValue : (DateOnly)subscription.StartDate,
                    ExpiryDate = String.IsNullOrEmpty(subscription.ExpiryDate.ToString()) ? DateOnly.MinValue : (DateOnly)subscription.ExpiryDate,
                    PriceAfterDiscount = subscription.PriceAfterDiscount,
                    TaxAmount = subscription.TaxAmount,
                    FinalAmount = subscription.FinalAmount
                });
                var result = new Paginated<GetSubscriptionDTO>
                {
                    dataArray = paginatedSubscriptionDTOAndTotalPages.Item1,
                    totalPages = paginatedSubscriptionDTOAndTotalPages.Item2
                };
                
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
