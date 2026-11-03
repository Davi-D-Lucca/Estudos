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
            // var livros = await _context.Livros.ToListAsync();
            // Include carrega o Autor junto; sem ele livro.Autor vem null depois do ToListAsync
            var livros = await _context.Livros.Include(l => l.Autor).ToListAsync();

            var response = livros.Select(livro => new LivroResponseDTO
            {
                Id = livro.Id,
                NameBook = livro.NameBook,
                Pages = livro.Pages,
                // NameArtist = livro.Autor.NameAutor,
                AutorId = livro.AutorId,
                NameAutor = livro.Autor.NameAutor,
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
            // // Sem essa checagem, um AutorId inexistente estoura erro de FK no SaveChanges (500)
            // var autorExiste = await _context.Autor.AnyAsync(a => a.Id == livroDto.AutorId);
            //
            // if (!autorExiste)
            //     return BadRequest(new { mensagem = "Autor não encontrado." });
            //
            // var livros = new Livros
            // {
            //     NameBook = livroDto.NameBook,
            //     Pages = livroDto.Pages,
            //     // Autor = livroDto.NameArtist,
            //     AutorId = livroDto.AutorId,
            //     Category = livroDto.Category
            // };

            // Busca ou cria o autor: AutorId usa um existente; NameAutor procura pelo nome e cria se não achar
            if (livroDto.AutorId is null && string.IsNullOrWhiteSpace(livroDto.NameAutor))
                return BadRequest(new { mensagem = "Informe AutorId ou NameAutor." });

            Autor? autor;

            if (livroDto.AutorId is not null)
            {
                autor = await _context.Autor.FindAsync(livroDto.AutorId);

                if (autor == null)
                    return BadRequest(new { mensagem = "Autor não encontrado." });
            }
            else
            {
                var nome = livroDto.NameAutor!.Trim();

                // Ignora maiúsculas: "machado de assis" acha "Machado de Assis"
                autor = await _context.Autor
                    .FirstOrDefaultAsync(a => a.NameAutor.ToLower() == nome.ToLower());

                // Não achou: cria um novo; o EF insere o autor antes do livro no mesmo SaveChanges
                autor ??= new Autor { NameAutor = nome };
            }

            var livros = new Livros
            {
                NameBook = livroDto.NameBook,
                Pages = livroDto.Pages,
                Autor = autor, // navegação em vez de AutorId: o autor novo ainda não tem Id
                Category = livroDto.Category
            };

            _context.Livros.Add(livros);
            await _context.SaveChangesAsync();

            // return CreatedAtAction(
            //     nameof(GetId),
            //     new { id = livros.Id },
            //     livros
            // );

            // Devolve DTO: a entidade agora tem livros.Autor.Livros apontando de volta e o JSON entraria em ciclo
            return CreatedAtAction(
                nameof(GetId),
                new { id = livros.Id },
                new LivroResponseDTO
                {
                    Id = livros.Id,
                    NameBook = livros.NameBook,
                    Pages = livros.Pages,
                    AutorId = autor.Id,
                    NameAutor = autor.NameAutor,
                    Category = livros.Category
                }
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
            // Trocar o autor do livro é trocar o AutorId; mexer em Autor.NameAutor renomearia o autor
            var autorExiste = await _context.Autor.AnyAsync(a => a.Id == livroDto.AutorId);

            if (!autorExiste)
                return BadRequest(new { mensagem = "Autor não encontrado." });

            livroExistente.Pages = livroDto.Pages;
            // livroExistente.Autor.NameAutor = livroDto.NameArtist;
            livroExistente.AutorId = livroDto.AutorId;
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

            // if (!String.IsNullOrEmpty(livro.Autor.NameAutor))
            //     livroExistente.Autor.NameAutor = livro.Autor.NameAutor;
            if (livro.AutorId > 0)
            {
                var autorExiste = await _context.Autor.AnyAsync(a => a.Id == livro.AutorId);

                if (!autorExiste)
                    return BadRequest(new { mensagem = "Autor não encontrado." });

                livroExistente.AutorId = livro.AutorId;
            }

            if(livro.Category != null)
                livroExistente.Category = livro.Category;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaginado(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? name = "")
        {

            var query = _context.Livros.AsQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => EF.Functions.ILike(x.NameBook, $"%{name}%"));
            }

            var total = await query.CountAsync();

            var livros = await query
                    .OrderBy(l => l.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(livro => new LivroResponseDTO
                    {
                        Id = livro.Id,
                        NameBook = livro.NameBook,
                        Pages = livro.Pages,
                        // NameArtist = livro.Autor.NameAutor,
                        AutorId = livro.AutorId,
                        NameAutor = livro.Autor.NameAutor,
                        Category = livro.Category
                    })
                    .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                dados = livros
            });
        }


    }
}
