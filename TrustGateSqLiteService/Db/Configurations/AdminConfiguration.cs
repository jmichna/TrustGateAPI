using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> entity)
    {
        entity.ToTable("Admin");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Password).IsRequired();
        entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
    }
}
