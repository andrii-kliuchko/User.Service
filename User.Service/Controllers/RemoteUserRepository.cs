using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Service.Models;

namespace User.Service.Controllers
{
    public class RemoteUserRepository : IUserRepository
    {
        public Task<IActionResult> Create(UserItem user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update(long id, UserItem user)
        {
            throw new NotImplementedException();
        }
    }
}
