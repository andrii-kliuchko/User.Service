using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Service.Models;

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
            if (user == null)
                return new NoContentResult();
            Log.Information("Received request to save user: {user}", user);
            string fileText;
            try
            {
                using (FileStream fileStream = GetStorageFileStreamToRead())
                    fileText = await ReadText(fileStream);
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
            List<UserItem> userList = ReadUserListFromJson(fileText);
            long nextId;
            if (userList == null)
            {
                userList = new List<UserItem>();
                nextId = 1;
                Log.Warning("File did not contain JSON, created new List");
            }
            else
            {
                userList = userList.OrderBy(o => o.Id).ToList();
                nextId = userList[userList.Count - 1].Id + 1;
                Log.Information("Text has been parsed to List");
            }
            user.Id = nextId;
            userList.Add(user);
            fileText = WriteUserListToJson(userList);
            using (FileStream fileStream = GetStorageFileStreamToWrite())
                WriteText(fileStream, fileText);
            return new OkResult();
        }

        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return new NoContentResult();
            string fileText;
            try
            {
                using (FileStream fileStream = GetStorageFileStreamToRead())
                    fileText = await ReadText(fileStream);
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
            List<UserItem> userList = ReadUserListFromJson(fileText);
            if (userList == null)
            {
                Log.Warning("File did not contain JSON, nothing to delete");
                return new NoContentResult();
            }
            for (int i = 0; i < userList.Count; i++)
                if (userList[i].Id == id)
                {
                    UserItem userToDelete = userList[i];
                    userList.RemoveAt(i);
                    fileText = WriteUserListToJson(userList);
                    using (FileStream fileStream = GetStorageFileStreamToWrite())
                        WriteText(fileStream, fileText);
                    Log.Information("User {userToDelete} has been removed from list", userToDelete);
                    return new OkResult();
                }
            return new NotFoundResult();
        }

        public async Task<IActionResult> DeleteAll()
        {
            List<UserItem> userList = new List<UserItem>();
            string fileText = WriteUserListToJson(userList);
            using (FileStream fileStream = GetStorageFileStreamToWrite())
                WriteText(fileStream, fileText);
            return new OkResult();
        }

        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return new NoContentResult();
            Log.Information("Received request to find user with id {id}", id);
            string fileText;
            try
            {
                using (FileStream fileStream = GetStorageFileStreamToRead())
                    fileText = await ReadText(fileStream);
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
            List<UserItem> userList = ReadUserListFromJson(fileText);
            if (userList == null)
            {
                Log.Warning("File did not contain JSON, nothing to find");
                return new NoContentResult();
            }
            else
            {
                foreach (UserItem user in userList)
                    if (user.Id.Equals(id))
                        return new OkObjectResult(user);
                return new NotFoundResult();
            }
        }

        public async Task<IActionResult> GetList()
        {
            string fileText;
            try
            {
                using (FileStream fileStream = GetStorageFileStreamToRead())
                    fileText = await ReadText(fileStream);
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
            List<UserItem> usersList = ReadUserListFromJson(fileText);
            return new OkObjectResult(usersList);
        }

        public async Task<IActionResult> Update(long id, UserItem newUser)
        {
            if (id < 1 || newUser == null)
                return new NoContentResult();
            Log.Information("Received request to update user with id {id}", id);
            string fileText;
            try
            {
                using (FileStream fileStream = GetStorageFileStreamToRead())
                    fileText = await ReadText(fileStream);
            }
            catch (FileNotFoundException)
            {
                return new NoContentResult();
            }
            List<UserItem> userList = ReadUserListFromJson(fileText);
            if (userList == null)
            {
                Log.Warning("File did not contain JSON, nothing to update");
                return new NoContentResult();
            }
            else
            {
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].Id == id)
                    {
                        newUser.Id = id;
                        userList[i] = newUser;
                        fileText = WriteUserListToJson(userList);
                        using (FileStream fileStream = GetStorageFileStreamToWrite())
                            WriteText(fileStream, fileText);
                        Log.Information("User {newUser} has been updated in list", newUser);
                        return new OkResult();
                    }
                }
                return new NotFoundResult();
            }
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

        private List<UserItem> ReadUserListFromJson(string jsonText)
        {
            return JsonConvert.DeserializeObject<List<UserItem>>(jsonText);
        }

        private string WriteUserListToJson(List<UserItem> userList)
        {
            return JsonConvert.SerializeObject(userList);
        }

        private FileStream GetStorageFileStreamToRead()
        {
            return new FileStream(_jsonFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        }

        private FileStream GetStorageFileStreamToWrite()
        {
            return new FileStream(_jsonFileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        }
    }
}
