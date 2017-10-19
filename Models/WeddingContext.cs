using Microsoft.EntityFrameworkCore;
 
namespace wedding.Models
{

    public class WeddingContext : DbContext
    {
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rsvp> RSVPs { get; set; }
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options) { }
    }
}