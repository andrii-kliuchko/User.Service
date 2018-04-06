using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using User.Service.Models;

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
        public async Task<IActionResult> GetAll()
        {
            return _repository.GetList().Result;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return _repository.Get(id).Result;
        }
        
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]UserItem user)
        {
            return _repository.Create(user).Result;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]UserItem user)
        {
            return _repository.Update(id, user).Result;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return _repository.Delete(id).Result;
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete()
        {
            return _repository.DeleteAll().Result;
        }
    }
}
