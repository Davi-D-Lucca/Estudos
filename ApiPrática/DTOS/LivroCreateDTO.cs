using System.ComponentModel.DataAnnotations;

namespace ApiPrática.DTOS
{
    public class LivroCreateDTO
    {
        [Required(ErrorMessage = "Digite o Nome do Livro!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do livro tem que ter no minimo 3 caracteres!")]
        public string? NameBook { get; set; }
        public string? NameArtist { get; set; }
        [Range(1, 1000, ErrorMessage = "Não pode ser menor que 0!")]
        public int Pages { get; set; }
        public List<string>? Category { get; set; }
    }
}
