using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string NameAuthor {  get; set; }
        public int Age { get; set; }
        public List<Books>? Books { get; set; }
    }
}
