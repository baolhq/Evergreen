using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository UserRepository, IMapper mapper)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsers()
        {
            var users =  _UserRepository.GetUsers();
            return Ok(_mapper.Map<List<Account>>(users));

            
        }




        [HttpGet("{id}")]
        [Authorize (Roles = "User,Admin")]
        public IActionResult GetUser(int id)
        {
            var user = _UserRepository.GetUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_mapper.Map<Account>(user));
        }






        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] UserDTO UserCreate)
        {
            if (UserCreate == null)
                return BadRequest(ModelState);

            var User = _UserRepository.GetUsers()
                .Where(c => c.Email.Trim().ToUpper() == UserCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (User != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserMap = _mapper.Map<UserDTO>(UserCreate);

            if (!_UserRepository.CreateUser(UserMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create User Successfully");
        }





        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult UpdateUser(int id, Account updatedUser)
        {
            var user = _UserRepository.GetUser(id);
            if (user == null)
                return BadRequest(ModelState);

            if (user.AccountId != updatedUser.AccountId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_UserRepository.UpdateUser(updatedUser, id))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



        [HttpDelete("{email}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {

            var user = _UserRepository.GetUser(id);
            if (user == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_UserRepository.DeleteUser(id))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
