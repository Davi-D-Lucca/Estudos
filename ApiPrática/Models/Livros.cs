using System.ComponentModel.DataAnnotations;

namespace ApiPrática.Models
{
    public class Livros
    {
        [Key]
        public int Id { get; set; }
        public string NameBook { get; set; } = string.Empty;
        public int Pages { get; set; }
        public Autor Autor { get; set; } = null!;
        public int AutorId { get; set; }
        public List<string>? Category { get; set; }
    }
}
