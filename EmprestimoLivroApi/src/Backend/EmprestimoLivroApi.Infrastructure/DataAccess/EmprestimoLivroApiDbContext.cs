using EmprestimoLivroApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Infrastructure.DataAccess
{
    public class EmprestimoLivroApiDbContext : DbContext
    {
        public EmprestimoLivroApiDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Loan> Loan { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Reader> Reader { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("biblioteca");

            modelBuilder.Entity<Author>(author =>
            {
                author.ToTable("Author");
                author.Property(e => e.Id).HasColumnName("id");
                author.Property(e => e.NameAuthor).HasColumnName("name");
                author.Property(e => e.Age).HasColumnName("age");

                author.HasMany(a => a.Books) // 1 autor tem vários livros
                         .WithOne(b => b.Author)   // cada livro tem 1 autor
                         .HasForeignKey(b => b.AuthorId); // a ligação é o AuthorId do Book
            });

        }

    }
}
