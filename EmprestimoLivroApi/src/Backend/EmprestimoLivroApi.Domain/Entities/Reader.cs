using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Entities
{
    public class Reader
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Loan> Loans { get; set; }
    }
}
