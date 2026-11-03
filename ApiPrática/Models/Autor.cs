using System.ComponentModel.DataAnnotations;

namespace ApiPrática.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }
        public string? NameAutor { get; set; }
        public List<Livros> Livros { get; set; } = new();

    }
}
