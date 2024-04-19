using MembershipPortal.API.ErrorHandling;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using MembershipPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static MembershipPortal.DTOs.ProductDTO;
using static MembershipPortal.DTOs.UserDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MembershipPortal.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberService _subscriberService;

        private readonly string tableName = "Subscriber";
        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }


        // GET: api/<SubscriberController>
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<SubscriberWithGenderViewModel>>> Get()
        //{
        //    try
        //    {
        //        var subscriberList = await _subscriberService.GetSubscribersAsync();
        //        if(subscriberList != null)
        //        {
        //            return Ok(subscriberList);
        //        }
        //        return NotFound();
        //        //if (subscriberDto.Count() != 0)
        //        //{

        //        //    return Ok(subscriberDto);
        //        //}
        //        //else
        //        //{
        //        //    return NotFound(MyException.DataNotFound( tableName));
        //        //}
        //    }catch (Exception ex)
        //    {
        //        return StatusCode(500, MyException.DataProcessingError(ex.Message));
        //    }
        //}
        public async Task<ActionResult<IEnumerable<GetSubscriberDTO>>> Get(string? sortColumn, string? sortOrder)
        {
            try
            {
                var getSubscribersDTOList = await _subscriberService.GetAllSortedSubscribers(sortColumn, sortOrder);
                if (getSubscribersDTOList != null)
                {
                    return Ok(getSubscribersDTOList);
                }
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetSubscriberDTO>>> Search(string search)
        {
            try
            {
                var subscriberDto = await _subscriberService.SearchAsyncAll(search);

                if (subscriberDto.Count() != 0)
                {

                    return Ok(subscriberDto);
                }
                else
                {
                    return NotFound(MyException.DataWithNameNotFound( tableName));
                }
            }catch (Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // GET api/<SubscriberController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSubscriberDTO>> Get(long id)
        {
            try
            {
                var subscriberDto = await _subscriberService.GetSubscriberAsync(id);
                if(subscriberDto != null)
                {
                    return Ok(subscriberDto);

                }
                else
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // POST api/<SubscriberController>
        [HttpPost]
        public async Task<ActionResult<GetSubscriberDTO>> Post([FromBody] CreateSubscriberDTO subscriberDTO)
        {
            try
            {
                var subscriberDto = await _subscriberService.CreateSubscriberAsync(subscriberDTO);

                return Ok(subscriberDto);
            }catch(Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }

        }

        // PUT api/<SubscriberController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GetSubscriberDTO>> Put(long id,[FromBody] UpdateSubscriberDTO subscriberDTO)
        {
            try
            {
                if (id != subscriberDTO.Id)
                {
                    return BadRequest(MyException.IdMismatch());
                }
                var subscriberDto = await _subscriberService.UpdateSubscriberAsync(id, subscriberDTO);

                if (subscriberDto != null)
                {
                    return Ok(subscriberDto);
                }
                else
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }
            }
            catch( Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // DELETE api/<SubscriberController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _subscriberService.DeleteSubscriberAsync(id);
                return Ok(result);
                //if (subscriberDto)
                //{
                //    return StatusCode(200, MyException.DataDeletedSuccessfully(tableName));
                //}
                //else
                //{
                //    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                //}
            }
            catch(Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }
        [HttpPost("paginated")]
        public async Task<ActionResult<Paginated<GetSubscriberDTO>>> GetPaginatedUserData(string? sortColumn, string? sortOrder, int page, int pageSize, [FromBody] GetSubscriberDTO subscriber)
        {
            try
            {
                var paginatedSubscriberDTOAndTotalPages = await _subscriberService.GetAllPaginatedSubscriberAsync(page, pageSize, new Subscriber()
                {
                    FirstName = subscriber.FirstName,
                    LastName = subscriber.LastName,
                    Email = subscriber.Email,
                    ContactNumber = subscriber.ContactNumber,
                    GenderId = subscriber.GenderId
                });
                var result = new Paginated<GetSubscriberDTO>
                {
                    dataArray = paginatedSubscriberDTOAndTotalPages.Item1,
                    totalPages = paginatedSubscriberDTOAndTotalPages.Item2
                };
                if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
                {
                    // Determine the sort order based on sortOrder parameter
                    bool isAscending = sortOrder.ToLower() == "asc";
                    switch (sortColumn.ToLower())
                    {
                        case "firstname":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.FirstName) : result.dataArray.OrderByDescending(s => s.FirstName);
                            break;
                        case "lastname":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.LastName) : result.dataArray.OrderByDescending(s => s.LastName);
                            break;
                        case "email":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.Email) : result.dataArray.OrderByDescending(s => s.Email);
                            break;
                        case "contactnumber":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.ContactNumber) : result.dataArray.OrderByDescending(s => s.ContactNumber);
                            break;
                        case "gendername":
                            result.dataArray = isAscending ? result.dataArray.OrderBy(s => s.GenderName) : result.dataArray.OrderByDescending(s => s.GenderName);
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
