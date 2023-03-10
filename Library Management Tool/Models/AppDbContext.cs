using Microsoft.EntityFrameworkCore;

namespace Library_Management_Tool.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookPublisher> BookPublishers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}
