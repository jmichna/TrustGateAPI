using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class ApiEndpointConfiguration : IEntityTypeConfiguration<ApiEndpoint>
{
    public void Configure(EntityTypeBuilder<ApiEndpoint> entity)
    {
        entity.ToTable("ApiEndpoint");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(e => e.HttpMethod)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(e => e.Route)
              .IsRequired()
              .HasMaxLength(300);

        entity.HasOne(e => e.Company)
              .WithMany(c => c.ApiEndpoints)
              .HasForeignKey(e => e.CompanyId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
