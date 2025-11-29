using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;
public class ApiTokenConfiguration : IEntityTypeConfiguration<ApiToken>
{
    public void Configure(EntityTypeBuilder<ApiToken> entity)
    {
        entity.ToTable("ApiToken");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Value)
              .IsRequired();

        entity.Property(e => e.Status)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(e => e.CreatedAt)
              .IsRequired();
    }
}
