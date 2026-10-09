# Estudos — ApiPrática (CRUD de Livros)

> Marque `[x]` ao concluir. A parte prática vem depois da teoria de cada bloco, mas pode misturar.

---

## 1. Checklist de estudo (teoria)

### Fundamentos da API

- [X] Injeção de dependência: Scoped vs Transient vs Singleton (e por que o DbContext é Scoped)
- [ ] Pipeline de middlewares e a importância da ordem (`UseHttpsRedirection`, `UseAuthorization`, `MapControllers`)
- [ ] Diferença entre `UseAuthentication` e `UseAuthorization`
- [ ] O que o `[ApiController]` faz sozinho (validação automática, `[FromBody]` inferido, `ProblemDetails`)
- [ ] Rotas: `[Route("api/[controller]")]`, `[HttpGet("{id}")]`, route constraints (`{id:int}`)
- [ ] `IActionResult` vs `ActionResult<T>` e `[ProducesResponseType]`
- [ ] Códigos HTTP: 200, 201 (+ header Location), 204, 400, 404, 409
- [ ] async/await: o que acontece por baixo no `await` do EF

### EF Core + PostgreSQL

- [ ] DbContext, DbSet e change tracker
- [ ] `Find` vs `FirstOrDefault` vs `SingleOrDefault`
- [ ] `AsNoTracking` nas leituras
- [ ] Migrations: `add-migration`, `update-database`, `remove-migration`, o que é o `ModelSnapshot`
- [ ] `List<string>` mapeado como `text[]` (Npgsql) vs tabela de Categoria com N:N
- [ ] `IQueryable` vs `IEnumerable` (quando a query vira SQL)
- [ ] Relacionamentos 1:N e N:N no EF (Fluent API vs convenção)

### Qualidade e boas práticas

- [ ] Nullable reference types: `string?`, `required`, `= string.Empty`
- [ ] DTOs (Create / Update / Response) e overposting
- [ ] Validação: Data Annotations (`[Required]`, `[Range]`, `[StringLength]`) e FluentValidation
- [ ] Tratamento global de erros: `IExceptionHandler` + `ProblemDetails`
- [ ] User Secrets e variáveis de ambiente (nada de senha no Git)
- [ ] OpenAPI nativo (`AddOpenApi`) vs Swashbuckle (só a UI aqui)
- [ ] `JsonNumberHandling.Strict`: o que muda no JSON
- [ ] Convenções de nomenclatura do C# (`Livro` no singular, idioma consistente)

### Extras (depois do básico)

- [ ] Paginação, filtro e ordenação via query string
- [ ] Testes de integração com `WebApplicationFactory`
- [ ] Teste unitário de regra de negócio
- [ ] Docker Compose (API + PostgreSQL)

---

## 2. Parte prática

Cada item tem um objetivo claro. Faça na ordem; um commit por item.

### P1 — Completar o CRUD (PUT)

- [X] Criar `PUT api/livros/{id}` que atualiza um livro existente
- [X] Retornar `404` se não existir e `204` se atualizar
- [X] Bônus: tentar também `PATCH` e comparar com `PUT`

### P2 — DTOs

- [ ] Criar `LivroCreateDto`, `LivroUpdateDto` e `LivroResponseDto`
- [ ] Parar de receber/devolver a entidade `Livros` direto no controller
- [ ] Conferir que o cliente não consegue mais enviar o `Id` no POST

### P3 — Validação

- [ ] `NameBook` obrigatório e com tamanho máximo
- [ ] `Pages` maior que 0
- [ ] `Category` com pelo menos 1 item
- [ ] Testar no Swagger/.http e ver o `400` com `ProblemDetails`

### P4 — GET com filtro e paginação

- [ ] `GET api/livros?categoria=ficcao&page=1&pageSize=10`
- [ ] Filtro por nome (contém) e ordenação
- [ ] Usar `AsNoTracking` e montar a query em `IQueryable` antes do `ToListAsync`
- [ ] Devolver também o total de itens

### P5 — Autor como entidade

- [ ] Trocar `NameArtist` por uma entidade `Autor` (relação 1:N com `Livro`)
- [ ] Criar nova migration e aplicar
- [ ] Ajustar DTOs e endpoints (ex.: `GET api/autores/{id}/livros`)

### P6 — Configuração segura

- [ ] Mover a connection string para User Secrets
- [ ] Remover a connection string comentada antiga do `appsettings.json`
- [ ] Reescrever o `ApiPrática.http` com requests reais dos seus endpoints (apagar o `weatherforecast`)

### P7 — Erros e testes

- [ ] `IExceptionHandler` global devolvendo `ProblemDetails`
- [ ] 1 teste de integração (`WebApplicationFactory`) cobrindo POST + GET
- [ ] 1 teste unitário simples

### P8 — Docker

- [ ] `docker-compose.yml` com PostgreSQL
- [ ] `Dockerfile` da API e subir tudo com `docker compose up`

---

## 3. Anotações — o que eu estudei

> Copie o modelo abaixo para cada sessão de estudo (a mais recente fica em cima).

### Modelo

```
### AAAA-MM-DD — assunto
- O que estudei:
- O que entendi (com minhas palavras):
- O que ainda não ficou claro:
- Onde apliquei no código (arquivo/commit):
```
