using Links.Models;
using Microsoft.EntityFrameworkCore;

namespace Links.Data
{
    public class LinkContext : DbContext
    {
        public LinkContext(DbContextOptions<LinkContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().HasIndex(l => l.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Link> ShortLinks { get; set; }
    }
}
