using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> entity)
    {
        entity.ToTable("Company");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
        entity.Property(e => e.CompanyInitials).IsRequired().HasMaxLength(20);
        entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(200);
        entity.Property(e => e.ProjectId).IsRequired();
    }
}
