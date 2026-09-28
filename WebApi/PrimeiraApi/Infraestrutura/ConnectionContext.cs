using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Domain.Model.CompanyAggregate;
using PrimeiraApi.Domain.Model.EmployeeAggregate;

namespace PrimeiraApi.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> company { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                "Server=localhost;" +
                "Port=3305;Database=postgres;" +
                "User Id=postgres;" +
                "Password=password"
            );

    }
}
