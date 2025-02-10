using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingAlliance.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketingAlliance.Infrastructure.Configurations
{
    public class PartnershipApplicationConfiguration : IEntityTypeConfiguration<PartnershipApplication>
    {
        public void Configure(EntityTypeBuilder<PartnershipApplication> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.Patronymic)
                .IsRequired(false);

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Pharmacy).IsRequired();
            builder.Property(e => e.NumberOfRetailPoints).IsRequired();
            builder.Property(e => e.INN).HasMaxLength(12).IsRequired();
            builder.Property(e => e.ContactPhoneNumber).HasMaxLength(10).IsRequired();
            builder.Property(e => e.Comment)
                .IsRequired(false);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}