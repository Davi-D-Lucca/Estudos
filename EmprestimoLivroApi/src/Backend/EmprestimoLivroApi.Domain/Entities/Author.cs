using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string NameAuthor { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<Book> Books { get; set; } = [];
    }
}
