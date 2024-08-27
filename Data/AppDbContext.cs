using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
               .HasMany(u => u.Tasks)
               .WithOne()
               .HasForeignKey(ut => ut.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()"); modelBuilder.Entity<User>();

            modelBuilder.Entity<UserTask>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()"); modelBuilder.Entity<UserTask>();

        }

    }
}
