using System.Reflection;
using MarketingAlliance.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketingAlliance.Infrastructure.Data
{
    public class MarketingAllianceContext : DbContext
    {
        public MarketingAllianceContext(DbContextOptions<MarketingAllianceContext> options) : base(options)
        {
            FeedbackMessages = Set<FeedbackMessage>();
            PartnershipApplications = Set<PartnershipApplication>();
        }

        public DbSet<FeedbackMessage> FeedbackMessages { get; set; }
        public DbSet<PartnershipApplication> PartnershipApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}