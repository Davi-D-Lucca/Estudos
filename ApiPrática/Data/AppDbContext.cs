using ApiPrática.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPrática.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Livros> Livros { get; set; }
    }
}
