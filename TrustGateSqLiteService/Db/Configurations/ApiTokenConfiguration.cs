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

        entity.Property(e => e.Token)
              .IsRequired();

        entity.Property(e => e.ExpiresAt)
              .IsRequired();

        entity.Property(e => e.IsActive)
              .IsRequired();

        entity.HasOne(e => e.Project)
          .WithMany(p => p.ApiTokens)
          .HasForeignKey(e => e.ProjectId)
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasData(
            new ApiToken
            {
                Id = 1,
                Token = "DEMO_API_TOKEN_123",
                ExpiresAt = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                ProjectId = 1
            }
        );
    }
}
