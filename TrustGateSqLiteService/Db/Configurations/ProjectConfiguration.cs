using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> entity)
    {
        entity.ToTable("Project");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(200);

        entity.HasIndex(e => new { e.CompanyId, e.Name })
              .IsUnique();

        entity.HasData(
            new Project
            {
                Id = 1,
                Name = "Main API",
                CompanyId = 1
            },
            new Project
            {
                Id = 2,
                Name = "Internal API",
                CompanyId = 1
            }
        );
    }
}
