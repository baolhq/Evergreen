using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }



        [HttpGet]
        public ActionResult GetUsers()
        {
            var u = _userRepository.GetUsers();
            var m = _mapper.Map<IEnumerable<UserDTO>>(u);
            return Ok(m);
        }



        [HttpGet("{id}")]
        public ActionResult GetUserById(int id)
        {
            var u = _userRepository.GetUserById(id);
            var m = _mapper.Map<UserDTO>(u);
            return Ok(m);
        }



        [HttpPost]
        public ActionResult<UserDTO> SaveUser(UserDTO u)
        {
            var user = _mapper.Map<Account>(u);

            _userRepository.SaveUser(user);
            return Ok(user);
        }



        [HttpPut("{id}")]
        public ActionResult UpdateUser(UserDTO u)
        {
            var user = _mapper.Map<Account>(u);
            _userRepository.UpdateUser(user);
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();
            _userRepository.DeleteUser(user);
            return NoContent();
        }



        [HttpPost("GetUserByEmail")]
        public ActionResult<Account> GetUserByEmail(string email)
        {
            var u = _userRepository.GetUserByEmail(email);
            var m = _mapper.Map<UserDTO>(u);
            return Ok(m);
        }
    }
}
