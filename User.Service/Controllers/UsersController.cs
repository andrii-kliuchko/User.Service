using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return _repository.GetList().Result;
        }
        
        [HttpGet("get/{id}")]
        public string Get(int id)
        {
            return Environment.GetEnvironmentVariable("STORAGE_MODE");
        }

        // POST api/<controller>
        [HttpPost("create")]
        public void Post([FromBody]UserItem user)
        {
            _repository.Create(user);
            Log.Information(String.Format("Created user ({0})", user));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
