using Microsoft.EntityFrameworkCore;

namespace User.Service.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<UserItem> Users { get; set; }
    }
}
