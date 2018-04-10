using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using User.Service.Models;

namespace User.Service.Controllers
{
    public interface IUserRepository
    {
        Task<IActionResult> GetList();
        Task<IActionResult> Get(long id);
        Task<IActionResult> Create(UserItem user);
        Task<IActionResult> Update(long id, UserItem user);
        Task<IActionResult> Delete(long id);
        Task<IActionResult> DeleteAll();
    }
}
