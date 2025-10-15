using Microsoft.EntityFrameworkCore;
using TrustGateCore.Models;

namespace TrustGateSqlLiteService.Db
{
    public class SqlDbContext : DbContext
    {
        //public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source= C:\\Users\\jmich\\OneDrive\\Pulpit\\trustgate.db");
        }

        // DbSety dla wszystkich tabel z UML
        public DbSet<ControllerUser> ControlerUsers { get; set; }
        public DbSet<ControllerAuthorization> ControllerAuthorizations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ ControlerUser
            modelBuilder.Entity<ControllerUser>(entity =>
            {
                entity.ToTable("ControlerUser");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Initials).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Password).IsRequired();
            });

            // ✅ Company
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CompanyInitials).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ProjectId).IsRequired();
            });

            // ✅ Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            });

            // ✅ ControllerAuthorization
            modelBuilder.Entity<ControllerAuthorization>(entity =>
            {
                entity.ToTable("ControllerAuthorization");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ControllerName).HasMaxLength(200);
                entity.Property(e => e.Generic).IsRequired();

                // Relacja: Authorization → User (wiele do jednego)
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Authorizations)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacja: Authorization → Company (wiele do jednego)
                entity.HasOne(e => e.Company)
                      .WithMany(c => c.Authorizations)
                      .HasForeignKey(e => e.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
