using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Service.Models;

namespace User.Service.Controllers
{
    public interface IUserRepository
    {
        Task<IActionResult> GetList();
        Task<IActionResult> Get(long id);
        Task<IActionResult> Create(UserItem user);
        Task<IActionResult> Update(long id, UserItem user);
        Task<IActionResult> Delete(UserItem user);
    }
}
