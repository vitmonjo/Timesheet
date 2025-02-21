namespace TimesheetAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using TimesheetAPI.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
        public DbSet<UserClient> UserClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite primary key for UserClient
            modelBuilder.Entity<UserClient>()
                .HasKey(uc => new { uc.UserId, uc.ClientId });

            // Define relationships
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasOne(p => p.Project)
                .WithMany(c => c.Tasks)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
