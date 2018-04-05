using Microsoft.EntityFrameworkCore;
using User.Service.Models;

namespace User.Service.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<UserItem> Users { get; set; }
    }
}
