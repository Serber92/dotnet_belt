using Microsoft.EntityFrameworkCore;
using Belt.Models;
 
namespace ContextNamespace.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<_Activity> Activities { get; set; }
        public DbSet<Participant> Participants { get; set; }

    }
}