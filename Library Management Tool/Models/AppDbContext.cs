using Microsoft.EntityFrameworkCore;

namespace Library_Management_Tool.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasOne(b => b.Publisher).WithMany(p => p.Books).HasForeignKey(b => b.PublisherId);
            modelBuilder.Entity<Issue>().HasOne(i => i.Member).WithMany(m => m.Issues).HasForeignKey(i => i.MemberId);
            modelBuilder.Entity<Issue>().HasOne(i => i.Book).WithMany(b => b.Issues).HasForeignKey(i => i.BookId);
        }
    }
}
