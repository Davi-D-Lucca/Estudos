using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumberPages { get; set; }
        public string? CapaLivro { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public List<Loan> Loans { get; set; }
    }
}
