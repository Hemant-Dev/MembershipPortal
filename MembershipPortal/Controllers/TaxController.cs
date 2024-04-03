﻿using System;
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

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        // GET: api/Tax
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTaxDTO>>> GetTaxesAsync()
        {
            var taxDTOList = await _taxService.GetTaxesAsync();
            if(taxDTOList != null)
            {
                return Ok(taxDTOList);
            }
            return NoContent();
        }

        // GET: api/Tax/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tax>> GetTaxByIdAsync(long id)
        {
            var taxDTO = await _taxService.GetTaxByIdAsync(id);

            if (taxDTO == null)
            {
                return NotFound(id);
            }

            return Ok(taxDTO);
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
                return await _taxService.UpdateTaxAsync(id, taxDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Tax
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tax>> PostTaxAsync(CreateTaxDTO taxDTO)
        {
            var result = await _taxService.CreateTaxAsync(taxDTO);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
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
                return Ok(false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool TaxExists(long id)
        {
            var result = _taxService.GetTaxByIdAsync(id);
            if(result != null)
            {
                return true;
            }
            return false;
        }
    }
}