namespace ApiPrática.DTOS
{
    public class LivroUpdateDTO
    {
        public string? NameBook { get; set; }
        public int AutorId { get; set; }
        public int Pages { get; set; }
        public List<string>? Category { get; set; }
    }
}
