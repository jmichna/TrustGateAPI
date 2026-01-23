using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> entity)
    {
        entity.ToTable("Company");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(e => e.Initials)
              .IsRequired()
              .HasMaxLength(20);

        entity.HasMany(e => e.Projects)
              .WithOne(p => p.Company)
              .HasForeignKey(p => p.CompanyId);

        entity.HasData(
            new Company
            {
                Id = 1,
                Name = "TrustGate Demo Company",
                Initials = "TGD"
            }
        );
    }
}