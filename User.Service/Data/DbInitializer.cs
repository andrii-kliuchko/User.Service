using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Service.Models;

namespace User.Service.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new UserItem[]
            {
                new UserItem { FirstName = "John", LastName = "Ive" },
                new UserItem { FirstName = "Tim", LastName = "Cook" },
                new UserItem { FirstName = "Steve", LastName = "Jobs" }
            };

            foreach (UserItem user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }
    }
}
