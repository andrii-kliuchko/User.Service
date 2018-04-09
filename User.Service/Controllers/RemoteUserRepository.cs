﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using User.Service.Models;
using User.Service.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace User.Service.Controllers
{
    public class RemoteUserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public RemoteUserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(UserItem user)
        {
            if (user == null)
                return new NoContentResult();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new OkObjectResult(user.Id);
        }

        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return new NoContentResult();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return new NoContentResult();
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
        }

        public async Task<IActionResult> DeleteAll()
        {
            Log.Information("Received request to delete all users");
            string tableName = _context.Model.FindEntityType(typeof(UserItem)).Relational().TableName;
            Log.Information("Identified table name to clear: {tableName}", tableName);
            int result = await _context.Database.ExecuteSqlCommandAsync(String.Format("DELETE from {0}", tableName));
            Log.Information("Return code from Database: {result}", result);
            if (result != 0)
                return new OkObjectResult(result);
            else
                return new NoContentResult();
        }

        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return new NoContentResult();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return new NoContentResult();
            else
                return new OkObjectResult(user);
        }

        public async Task<IActionResult> GetList()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new NoContentResult();
            else
                return new OkObjectResult(users);
        }

        public async Task<IActionResult> Update(long id, UserItem userNew)
        {
            if (userNew == null || id < 1)
                return new NoContentResult();
            var userInDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (userInDb == null)
                return new NotFoundResult();
            userInDb.FirstName = userNew.FirstName;
            userInDb.LastName = userNew.LastName;
            _context.Users.Update(userInDb);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
