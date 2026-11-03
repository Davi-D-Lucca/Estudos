using ApiPrática.Data;
using ApiPrática.DTOS;
using ApiPrática.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPrática.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AutorController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}/livros")]
        public async Task<ActionResult> GetAutor(int id)
        {
            var autor = await _context.Autor
                .Where(a => a.Id == id)
                .Select(a => new ListLivrosAutorDTO
                {
                    Id = a.Id,
                    NameAutor = a.NameAutor,
                    Livros = a.Livros.Select(l => new LivroResumoDTO
                    {
                        Id = l.Id,
                        NameBook = l.NameBook,
                        Pages = l.Pages
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (autor == null)
                return NotFound(new { mensagem = "Autor não encontrado." });

            return Ok(autor);            
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetAutorPaginado(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? name = "")
        {
            var query = _context.Autor.AsQueryable().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query
                    .Where(x => EF.Functions.ILike(x.NameAutor, $"%{name}%"));
            }

            var total = await query.CountAsync();
            var autores = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ListLivrosAutorDTO
                {
                    Id = a.Id,
                    NameAutor = a.NameAutor,
                    Livros = a.Livros.Select(l => new LivroResumoDTO
                    {
                        Id = l.Id,
                        NameBook = l.NameBook,
                        Pages = l.Pages
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                dados = autores
            });
        }
    }
}
