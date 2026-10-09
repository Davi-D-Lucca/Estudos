namespace ApiPrática.Models
{
    public class Livros
    {
        public int Id { get; set; }
        public string NameBook { get; set; } = string.Empty;
        public int Pages { get; set; }
        public string NameArtist { get; set; } = string.Empty;
        public List<string>? Category { get; set; }
    }
}
