using Microsoft.EntityFrameworkCore;

namespace VerifyIndia
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApiLogs> ApiLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApiLogs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ApiEndpoint).HasMaxLength(500);
                entity.Property(e => e.HttpMethod).HasMaxLength(10);
            });
        }
    }
}
