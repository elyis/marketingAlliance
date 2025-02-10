using MarketingAlliance.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketingAlliance.Infrastructure.Configurations
{
    public class FeedbackMessageConfiguration : IEntityTypeConfiguration<FeedbackMessage>
    {
        public void Configure(EntityTypeBuilder<FeedbackMessage> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FirstName)
                .IsRequired();

            builder.Property(e => e.LastName)
                .IsRequired();

            builder.Property(e => e.Patronymic)
                .IsRequired(false);

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Question)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}