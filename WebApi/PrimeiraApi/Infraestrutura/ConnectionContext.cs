using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Domain.Model;

namespace PrimeiraApi.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                "Server=localhost;" +
                "Port=3305;Database=postgres;" +
                "User Id=postgres;" +
                "Password=password"
            );

    }
}
