using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AirCC.Portal.EntityFramework
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AirCCDbContext>
    {
        public AirCCDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirCCDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=AirCC;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AirCCDbContext(optionsBuilder.Options, null);
        }
    }
}
