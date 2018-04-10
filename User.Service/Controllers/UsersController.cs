using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using User.Service.Models;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.Service.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("")]
        public IActionResult GetAll()
        {
            return _repository.GetList().Result;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return _repository.Get(id).Result;
        }
        
        [HttpPost("")]
        public IActionResult Post([FromBody]UserItem user)
        {
            Uri hostUri = new Uri(HttpContext.Request.Host.Value);
            return _repository.Create(user).Result;
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]UserItem user)
        {
            return _repository.Update(id, user).Result;
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return _repository.Delete(id).Result;
        }

        [HttpDelete("")]
        public IActionResult Delete()
        {
            return _repository.DeleteAll().Result;
        }
    }
}
