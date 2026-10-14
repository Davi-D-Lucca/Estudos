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

### Desafios extras — Web API
- [ ] Padrões REST: Modelo de Maturidade de Richardson (níveis 0 a 3) e o que é HATEOAS
- [ ] Autenticação por token: JWT (login, claims, expiração, assinatura) e `[Authorize]`
- [ ] Diferença entre autenticação e autorização (e `UseAuthentication` antes de `UseAuthorization`)
- [ ] Middlewares customizados para interceptar requests: `Use` vs `Run` vs `Map`, e a ordem no pipeline

### Desafios extras — .NET 6+
- [ ] Tempos de vida: Singleton vs Scoped vs Transient, e quando usar cada um para criar serviços eficientes
- [ ] Armadilha da dependência capturada (Singleton dependendo de Scoped) e `IServiceScopeFactory`
- [ ] Reflection para descobrir e registrar serviços automaticamente (varredura de assembly)
- [ ] Interfaces vs classes abstratas: quando usar cada uma e como organizar a estrutura de serviços
- [ ] Integração com serviço de terceiros: e-mail (SMTP/MailKit/SendGrid), Zapier (webhook), serviços Google, captcha (reCAPTCHA, hCaptcha ou Turnstile)
- [ ] `IHttpClientFactory` para chamar APIs externas
- [ ] Front-end separado: o que é CORS, por que o navegador bloqueia e como liberar com `AddCors`
- [ ] HTTPS e certificados: certificado de desenvolvimento (`dotnet dev-certs https --trust`), HSTS e redirecionamento

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

### P9 — REST nível 3 (HATEOAS)
- [ ] Revisar o que a API já cumpre do nível 2 (verbos e códigos HTTP corretos)
- [ ] Fazer `GET api/livros/{id}` devolver o livro com uma lista `links` (`self`, `update`, `delete`, `todos`)
- [ ] Gerar as URLs com `Url.Action`/`LinkGenerator`, sem escrever texto fixo
- [ ] Fazer o `GET api/livros` devolver os links de cada item

### P10 — Autenticação com token (JWT)
- [ ] Criar `POST api/auth/login` que valida um usuário simples e devolve um token JWT
- [ ] Guardar a chave de assinatura no User Secrets (nunca no código)
- [ ] Proteger POST, PUT, PATCH e DELETE com `[Authorize]`, deixando os GET públicos
- [ ] Testar no Postman: sem token (401) e com `Authorization: Bearer <token>` (200)
- [ ] Bônus: papéis (roles) — só "admin" pode excluir

### P11 — Middlewares
- [ ] Criar `RequestLoggingMiddleware`: loga método, rota, status e tempo da requisição
- [ ] Criar um middleware que adiciona o header `X-Correlation-Id` em toda resposta
- [ ] Criar um middleware de tratamento de erros (devolve `ProblemDetails` em vez de estourar exceção)
- [ ] Registrar na ordem certa no `Program.cs` e testar o que muda se trocar a ordem

### P12 — Singleton, Scoped e Transient
- [ ] Criar 3 serviços simples, cada um com um `Guid` gerado no construtor, registrados como Singleton, Scoped e Transient
- [ ] Injetar os 3 no controller e também em um segundo serviço, e devolver os Guids numa rota de teste
- [ ] Chamar a rota duas vezes e anotar quais Guids mudam e quais não
- [ ] Provocar de propósito uma dependência capturada (Singleton usando Scoped) e ver o erro

### P13 — Reflection, interfaces e classes abstratas
- [ ] Criar `INotificador` (interface) e `NotificadorBase` (classe abstrata com o código comum)
- [ ] Criar 2 implementações (ex.: `NotificadorEmail` e `NotificadorLog`)
- [ ] Registrar todas as implementações de `INotificador` por reflection, varrendo o assembly, sem registrar uma a uma
- [ ] Injetar `IEnumerable<INotificador>` e chamar todas ao criar um livro

### P14 — Serviço de terceiros
- [ ] Enviar um e-mail ao cadastrar um livro (MailKit/SMTP de teste, como Mailtrap, ou SendGrid)
- [ ] Alternativa: disparar um webhook do Zapier a cada livro criado
- [ ] Usar `IHttpClientFactory` para chamadas HTTP externas e User Secrets para chaves/senhas
- [ ] Bônus: validar um captcha (reCAPTCHA ou Turnstile) no endpoint de login antes de gerar o token

### P15 — Front-end separado, CORS e HTTPS
- [ ] Criar um front simples (HTML + JS ou React/Vite) em outra porta que consome `GET api/livros`
- [ ] Ver o erro de CORS no console do navegador e entender a mensagem
- [ ] Configurar `AddCors` com uma policy que libera só a origem do front e usar `UseCors`
- [ ] Rodar a API em HTTPS com o certificado de desenvolvimento confiável (`dotnet dev-certs https --trust`)
- [ ] Fazer o front chamar a API em HTTPS e enviar o token JWT no header

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
