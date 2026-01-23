using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrustGateAPI.Enums;
using TrustGateCore.Models;

namespace TrustGateSqLiteService.Db.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("ControlerUser");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Initials).IsRequired().HasMaxLength(20);
        entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
        entity.HasIndex(e => e.Login).IsUnique();
        entity.Property(e => e.PasswordHash).IsRequired();
        entity.Property(e => e.Role).IsRequired();
        entity.Property(e => e.CompanyId).IsRequired(false);

        entity.HasOne(e => e.Company)
              .WithMany(c => c.Users)
              .HasForeignKey(e => e.CompanyId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasData(new User
            {
            Id = 1,
            Name = "Administrator",
            Initials = "ADM",
            Login = "admin",
            PasswordHash = "$2a$12$6EiIeHbOJ5v4kFaDMqiRtu09ohesnLa4WYsiWBvVWfkf3atav4AJK",
            Role = UserRole.Admin
        });

        entity.HasData(
            new User
            {
                Id = 2,
                Name = "Demo Company User",
                Initials = "DCU",
                Login = "company",
                PasswordHash = "$2a$12$6EiIeHbOJ5v4kFaDMqiRtu09ohesnLa4WYsiWBvVWfkf3atav4AJK",
                Role = UserRole.Company,
                CompanyId = 1
            }
        );

    }
}
