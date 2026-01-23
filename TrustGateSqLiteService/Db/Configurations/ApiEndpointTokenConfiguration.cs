using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class ApiEndpointTokenConfiguration : IEntityTypeConfiguration<ApiEndpointToken>
{
    public void Configure(EntityTypeBuilder<ApiEndpointToken> entity)
    {
        entity.ToTable("ApiEndpointToken");

        entity.HasKey(e => new { e.ApiEndpointId, e.ApiTokenId });

        entity.HasOne(e => e.ApiEndpoint)
              .WithMany()
              .HasForeignKey(e => e.ApiEndpointId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.ApiToken)
              .WithMany(t => t.ApiEndpointTokens)
              .HasForeignKey(e => e.ApiTokenId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasData(
            new ApiEndpointToken
            {
                ApiTokenId = 1,
                ApiEndpointId = 1
            },
            new ApiEndpointToken
            {
                ApiTokenId = 1,
                ApiEndpointId = 2
            }
        );

    }
}
