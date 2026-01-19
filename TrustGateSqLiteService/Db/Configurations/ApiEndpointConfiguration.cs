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

        entity.HasOne(e => e.Project)
              .WithMany(p => p.ApiEndpoints)
              .HasForeignKey(e => e.ProjectId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasData(
            new ApiEndpoint
            {
                Id = 1,
                Name = "Get Users",
                HttpMethod = "GET",
                Route = "/api/users",
                ProjectId = 1
            },
            new ApiEndpoint
            {
                Id = 2,
                Name = "Create User",
                HttpMethod = "POST",
                Route = "/api/users",
                ProjectId = 1
            },
            new ApiEndpoint
            {
                Id = 3,
                Name = "Get Orders",
                HttpMethod = "GET",
                Route = "/api/orders",
                ProjectId = 2
            }
        );
    }
}
