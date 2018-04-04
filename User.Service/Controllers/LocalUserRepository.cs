using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Service.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Serilog;

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

        public async Task<IActionResult> Create(UserItem user)
        {
            Log.Information("Received UserItem to save: {user}", user);
            Log.Information("Opening json file {_jsonFileName}", _jsonFileName);
            string fileText;
            using (FileStream fileStream = new FileStream(_jsonFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                fileText = await ReadText(fileStream);
            }
            Log.Information("Text from fileStream has been read");
            List<UserItem> usersList = JsonConvert.DeserializeObject<List<UserItem>>(fileText);
            long nextId;
            if (usersList == null)
            {
                usersList = new List<UserItem>();
                nextId = 0;
                Log.Warning("File did not contain JSON, created new List");
            }
            else
            {
                usersList = usersList.OrderBy(o => o.Id).ToList();
                nextId = usersList[usersList.Count - 1].Id + 1;
                Log.Information("Text has been parsed to List");
            }
            user.Id = nextId;
            usersList.Add(user);
            fileText = JsonConvert.SerializeObject(usersList);
            Log.Information("List has been serialized to json text");
            using (FileStream fileStream = new FileStream(_jsonFileName, FileMode.Open, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                WriteText(fileStream, fileText);
            };
            return new OkResult();
        }

        public Task<IActionResult> Delete(UserItem user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetList()
        {
            try
            {
                using (FileStream fileStream = new FileStream(_jsonFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    string fileText = await ReadText(fileStream);
                    List<UserItem> usersList = JsonConvert.DeserializeObject<List<UserItem>>(fileText);
                    return new ObjectResult(usersList);
                };
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
        }

        public Task<IActionResult> Update(long id, UserItem user)
        {
            throw new NotImplementedException();
        }

        private async Task<string> ReadText(FileStream fileStream)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string readText = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                sb.Append(readText);
            }
            return sb.ToString();
        }

        private async void WriteText(FileStream fileStream, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);
            Log.Information("Starting to write {Length} bytes", encodedText.Length);
            await fileStream.WriteAsync(encodedText, 0, encodedText.Length);
        }
    }
}
