using MembershipPortal.API.ErrorHandling;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using MembershipPortal.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using static MembershipPortal.DTOs.ProductDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;
        public string tableName = "Gender";
        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }


        // GET: api/<GenderController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGenderDTO>>> Get()
        {
            try{
                var genders = await _genderService.GetGendersAsync();
                if(genders != null)
                {
                    return Ok(genders);
                }
                return NotFound();
                //if(genders.Count() != 0)
                //{
                  
                //     return Ok(genders);
                //}
                //else
                //{
                //    return NotFound(MyException.DataNotFound(tableName));
                //}

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetGenderDTO>>> Get(string search)
        {
            try
            {
                var genders = await _genderService.SearchGendersAsync(search);

                if (genders.Count() != 0)
                {

                    return Ok(genders);
                }
                else
                {
                    return NotFound(MyException.DataNotFound(tableName));
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // GET api/<GenderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetGenderDTO>> Get(long id)
        {
            try
            {

                var gender = await _genderService.GetGenderAsync(id);
                if (gender != null)
                {
                    return Ok(gender);
                }
                
                return NotFound(MyException.DataWithIdNotPresent(id, tableName));
             
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // POST api/<GenderController>
        [HttpPost]
        public async Task<ActionResult<GetGenderDTO>> Post([FromBody] CreateGenderDTO genderDTO)
        {
            try
            {
                var gender = await _genderService.CreateGenderAsync(genderDTO);
                return Ok(gender);
                
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // PUT api/<GenderController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GetGenderDTO>> Put(long id, UpdateGenderDTO genderDTO)
        {
            if(id != genderDTO.Id)
            {
               return BadRequest(MyException.IdMismatch());
            }

            try
            {
                var genders = await _genderService.UpdateGenderAsync(id, genderDTO);

                if (genders != null)
                {
                    return Ok(genders);
                }
                else
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // DELETE api/<GenderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var genderDeleted = await _genderService.DeleteGenderAsync(id);
                return Ok(genderDeleted);
                //if (genderDeleted)
                //{
                //    return StatusCode(200, MyException.DataDeletedSuccessfully(tableName));
                //}
                //else
                //{
                //    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                //}
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }


        [HttpPost("paginated")]
        public async Task<ActionResult<Paginated<GetProductDTO>>> GetPaginatedProductData(int page, int pageSize, [FromBody] GetGenderDTO gender)
        {
            try
            {
                var paginatedGenderDTOAndTotalPages = await _genderService.GetAllPaginatedGenderAsync(page, pageSize, new Gender()
                {
                    GenderName = gender.GenderName
                }); ; ;
                var result = new Paginated<GetGenderDTO>
                {
                    dataArray = paginatedGenderDTOAndTotalPages.Item1,
                    totalPages = paginatedGenderDTOAndTotalPages.Item2
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
