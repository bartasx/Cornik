using Curnik.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Curnik.Models.Database
{
    public partial class DatabaseContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.HasKey(e => e.RoomId);
            });

            modelBuilder.Entity<Statistics>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();
            });
        }
    }
}