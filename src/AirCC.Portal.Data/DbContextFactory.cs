using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.EntityFramework
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AirCCDbContext>
    {
        public AirCCDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirCCDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\sql2019;Database=AirCC;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AirCCDbContext(optionsBuilder.Options, null);
        }
    }
}
