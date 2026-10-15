using ApiPrática.Data;
using ApiPrática.DTOS;
using ApiPrática.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ApiPrática.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LivrosController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var livros = await _context.Livros.ToListAsync();

            var response = livros.Select(livro => new LivroResponseDTO
            {
                Id = livro.Id,
                NameBook = livro.NameBook,
                Pages = livro.Pages,
                NameArtist = livro.NameArtist,
                Category = livro.Category
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return NotFound(new { mensagem = "Livro não encontrado." });

            return Ok(livro);

        }

        [HttpPost]
        public async Task<IActionResult> Criar(LivroCreateDTO livroDto)
        {
            var livros = new Livros
            {
                NameBook = livroDto.NameBook,
                Pages = livroDto.Pages,
                NameArtist = livroDto.NameArtist,
                Category = livroDto.Category
            };

            _context.Livros.Add(livros);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetId),
                new { id = livros.Id },
                livros
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Del(int id)
        {
            var livro = await _context.Livros.FindAsync(id);

            if(livro == null)
                return NotFound(new { mensagem = "Livro não encontrado." });

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPorId(int id, LivroUpdateDTO livroDto)
        {
            var livroExistente = await _context.Livros.FindAsync(id);

            if(livroExistente == null)
                return NotFound(new { mensagem = "Contato não encontrado." });

            livroExistente.NameBook = livroDto.NameBook;
            livroExistente.Pages = livroDto.Pages;
            livroExistente.NameArtist = livroDto.NameArtist;
            livroExistente.Category = livroDto.Category;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> AtualizarParcialmente(int id, Livros livro)
        {
            var livroExistente = await _context.Livros.FindAsync(id);

            if(livroExistente == null)
                return NotFound(new { mensagem = "Contato não encontrado." });

            if (!String.IsNullOrEmpty(livro.NameBook)) 
                livroExistente.NameBook = livro.NameBook;

            if (livro.Pages > 0)
                livroExistente.Pages = livro.Pages;

            if (!String.IsNullOrEmpty(livro.NameArtist))
                livroExistente.NameArtist = livro.NameArtist;

            if(livro.Category != null)
                livroExistente.Category = livro.Category;

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
