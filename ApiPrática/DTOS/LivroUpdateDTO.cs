namespace ApiPrática.DTOS
{
    public class LivroUpdateDTO
    {
        public string? NameBook { get; set; }
        public string? NameArtist { get; set; }
        public int Pages { get; set; }
        public List<string>? Category { get; set; }
    }
}
