using AirCC.Portal.Domain;
using BCI.Extensions.EFCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AirCC.Portal.EntityFramework
{
    public class AirCCDbContext : DbContextBase<AirCCDbContext>
    {
        public AirCCDbContext(DbContextOptions<AirCCDbContext> options, IServiceProvider serviceProvider = null)
           : base(options, serviceProvider)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationConfiguration> ApplicationConfigurations { get; set; }
        public virtual DbSet<ApplicationConfigurationHistory> ApplicationConfigurationHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
