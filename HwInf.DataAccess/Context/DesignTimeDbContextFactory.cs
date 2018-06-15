using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HwInf.DataAccess.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HwInfContext>
    {
        public HwInfContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .Build();

            var builder = new DbContextOptionsBuilder<HwInfContext>();

            var connectionString = @"Server=127.0.0.1;Port=5432;Database=hwinf;User Id=hwinf;";

            builder.UseNpgsql(connectionString);

            return new HwInfContext(builder.Options);
        }
    }
}