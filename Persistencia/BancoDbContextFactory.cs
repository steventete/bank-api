using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistencia.Contextos;

namespace Persistencia
{
    public class BancoDbContextFactory : IDesignTimeDbContextFactory<BancoDbContext>
    {
        public BancoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BancoDbContext>();

            optionsBuilder.UseSqlServer("Server=DESKTOP-MVE3G23\\SQLEXPRESS;Database=BancoDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BancoDbContext(optionsBuilder.Options);
        }
    }
}
