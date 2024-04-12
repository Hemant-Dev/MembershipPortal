using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MembershipPortal.Data;
using MembershipPortal.Models;
using MembershipPortal.IServices;
using MembershipPortal.DTOs;
using MembershipPortal.API.ErrorHandling;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;
        private readonly string tableName = "Tax";


        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        // GET: api/Tax
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTaxDTO>>> GetTaxesAsync()
        {
            try
            {
                var taxDTOList = await _taxService.GetTaxesAsync();
                if(taxDTOList != null)
                {
                    return Ok(taxDTOList);
                }
                return null;
                //if (taxDTOList.Count() != 0)
                //{
                //    return Ok(taxDTOList);

                //}
                //return NotFound(MyException.DataNotFound(tableName));
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // GET: api/Tax/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaxDTO>> GetTaxByIdAsync(long id)
        {
            try
            {
                var taxDTO = await _taxService.GetTaxByIdAsync(id);

                if (taxDTO != null)
                {
                    return Ok(taxDTO);
                }
                return NotFound();

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // PUT: api/Tax/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<GetTaxDTO>> PutTaxAsync(long id, UpdateTaxDTO taxDTO)
        {
            if (id != taxDTO.Id)
            {
                return BadRequest("Id Mismatch");
            }

            try
            {
                var result =  await _taxService.UpdateTaxAsync(id, taxDTO);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }

        }

        // POST: api/Tax
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetTaxDTO>> PostTaxAsync([FromBody] CreateTaxDTO taxDTO)
        {
            try
            {

               // var test = new CreateTaxDTO()
                var result = await _taxService.CreateTaxAsync(taxDTO);
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

        // DELETE: api/Tax/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTax(long id)
        {
            try
            {
                var taxDTO = await _taxService.GetTaxByIdAsync(id);
                if (taxDTO != null)
                {
                    var result = await _taxService.DeleteTaxAsync(id);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

    }
}
