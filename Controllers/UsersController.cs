using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public User[] GetUserList()
        {
            return _userService.GetUsers();
        }


        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _userService.GetUser(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] User user)
        {
            _userService.AddUser(user);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            _userService.updateUser(id,user);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userService.DelUser(id);
        }
    }
}
