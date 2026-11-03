namespace ApiPrática.DTOS
{
    public class ListLivrosAutorDTO
    {
        public int Id { get; set; }
        public string NameAutor { get; set; } = string.Empty;
        public List<LivroResumoDTO> Livros { get; set; } = new();
    }
}
