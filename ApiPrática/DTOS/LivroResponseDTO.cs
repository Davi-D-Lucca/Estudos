namespace ApiPrática.DTOS
{
    public class LivroResponseDTO
    {
        public int Id { get; set; }
        public string? NameBook { get; set; }
        public int AutorId { get; set; }
        public string NameAutor { get; set; }
        public int Pages { get; set; }
        public List<string>? Category { get; set; }
    }
}
