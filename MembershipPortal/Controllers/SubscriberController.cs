using MembershipPortal.API.ErrorHandling;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static MembershipPortal.DTOs.ProductDTO;

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
        public async Task<ActionResult<IEnumerable<GetSubscriberDTO>>> Get()
        {
            try
            {
                var subscriberDto = await _subscriberService.GetSubscribersAsync();

                if (subscriberDto.Count() != 0)
                {

                    return Ok(subscriberDto);
                }
                else
                {
                    return NotFound(MyException.DataNotFound( tableName));
                }
            }catch (Exception ex)
            {
                return StatusCode(500, MyException.DataProcessingError(ex.Message));
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

                return StatusCode(200,subscriberDto);
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
                var subscriberDto = await _subscriberService.DeleteSubscriberAsync(id);

                if (subscriberDto)
                {
                    return StatusCode(200, MyException.DataDeletedSuccessfully(tableName));
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
    }
}
