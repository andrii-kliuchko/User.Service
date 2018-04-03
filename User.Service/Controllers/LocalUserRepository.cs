using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Service.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace User.Service.Controllers
{
    public class LocalUserRepository : IUserRepository
    {
        private IConfiguration _configuration;
        private string _jsonFileName;

        public LocalUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonFileName = _configuration["LocalStorageJsonFile"];
        }

        public Task<IActionResult> Create(UserItem user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(UserItem user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetList()
        {
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (StreamReader sr = new StreamReader(_jsonFileName))
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    List<UserItem> usersList = serializer.Deserialize<List<UserItem>>(jr);
                    return new Task<IActionResult>(() => new ObjectResult(usersList));
                }
            } catch (FileNotFoundException e)
            {
                return new Task<IActionResult>(() => new NoContentResult());
            }
        }

        public Task<IActionResult> Update(long id, UserItem user)
        {
            throw new NotImplementedException();
        }
    }
}
