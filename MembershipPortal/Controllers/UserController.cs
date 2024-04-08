﻿using MembershipPortal.API.ErrorHandling;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using static MembershipPortal.DTOs.UserDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        private readonly string tableName = "User";

        public UserController (IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> Get()
        {

            try
            {
                var users = await _userService.GetUsersAsync();
                if (users.Count() != 0)
                {

                    return Ok(users);
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

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> Get(long id)
        {
            try
            {
                var user = await _userService.GetUserAsync(id);
                if (user == null)
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }
                return Ok(user);

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> Post([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                var createUser = await _userService.CreateUserAsync(createUserDTO);
                return Ok(createUser);
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GetUserDTO>> Put(long id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            if(id!= updateUserDTO.Id)
            {
                return BadRequest(MyException.IdMismatch());

            }
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDTO);
                if (user != null)
                {
                    return Ok(user);
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

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(long id)
        {
            try
            {
                var userDeleted = await _userService.DeleteUserAsync(id);
                if (userDeleted)
                {
                    return StatusCode(200, MyException.DataDeletedSuccessfully(tableName));
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
    }
}
