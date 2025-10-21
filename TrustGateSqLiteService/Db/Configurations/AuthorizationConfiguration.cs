using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> entity)
    {
        entity.ToTable("ControllerAuthorization");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.ControllerName).HasMaxLength(200);
        entity.Property(e => e.Generic).IsRequired();

        entity.HasOne(e => e.User)
              .WithMany(u => u.Authorizations)
              .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Company)
              .WithMany(c => c.Authorizations)
              .HasForeignKey(e => e.CompanyId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
