# Diário de Estudos — Web API com .NET

> Projeto: `WebApi/PrimeiraApi` · .NET 10 · PostgreSQL (Docker) · Entity Framework

## Sumário

1. [Contexto](#contexto)
2. [Estruturando as pastas](#estruturando-as-pastas)
3. [Injeção de dependencia](#injeção-de-dependencia)
4. [ViewModel](#viewmodel)
5. [Envio de arquivos](#envio-de-arquivos)
6. [Rota de download](#rota-de-download)
7. [Segurança com JWT](#segurança-com-jwt)
8. [Arquitetura Web API](#arquitetura-web-api)
9. [Auto Mapper](#auto-mapper)
10. [Migrations](#migrations)
11. [Complementos (revisão do Claude)](#complementos-revisão-do-claude)

---

## Contexto

Aprendendo a criar 1 API no momento, base construida com .net 10 e banco de dados postgres rodando no docker.

Instalei 2 pacotes nuget, npgsql e o entity framework

---

## Estruturando as pastas

### Model

- Tem que ser montada desse jeito
- Colocar o Table para quando o nome da classe for diferente do nome da tabela no banco, criar as propriedades com o mesmo nome.
- Key é para definir a chave primária

```csharp
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        public int id { get; private set; }
        public string name { get; private set; }
        public int age { get; private set; }
        public string photo { get; private set; }

    }
}
```

### Interface - Repository

- Fiz a classe para poder Adicionar e Obter funcionários

### Infraestrutura

- Classe ConnectionContext, para mapear a conexão do banco
- Dbset mapeia, procura a tabela no banco e compara com a classe

```csharp
namespace PrimeiraApi.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}
```

- Configuração da conexão com o banco

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(
        "Server=localhost;" +
        "Port=3305;Database=postgres;" +
        "User Id=postgres;" +
        "Password=password"
    );
```

---

## Injeção de dependencia

É preciso definir issso no program

```csharp
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
```

---

## ViewModel

Criei 1 pasta chamado ViewModel, será ela que vou passar no controller para alimentar e passar na employeRepository

---

## Envio de arquivos

Para enviar arquivos, passar o `[FromForm]`, utilizar `IFormFile` e `Path.Combine` para salvar o caminho do arquivo e não em si salvar o arquivo ou converter ele, pegaremos ele, colocar num storage e salvar esse caminho, pequeno exemplo no Controller de como fazer para a API copiar o file para o storage

```csharp
var filePath = Path.Combine("Storage", employeeView.Photo.FileName);
using Stream fileStream = new FileStream(filePath, FileMode.Create);
employeeView.Photo.CopyTo(fileStream);
```

---

## Rota de download

Criado a rota de download da foto por ID, hoje o que utilizo é BO e DAO, então estou entendendo um pouco de como devo fazer e onde passar cada parametro para conseguir 1 resultado

> [!NOTE]
> Pelo que entendi também, o EntityFramework utiliza LINQ, não é necessario fazer ou injetar códigos SQL no código

---

## Segurança com JWT

Trabalhando com segurança agora na API, usaremos JWT, injetar 1 token de acesso

- Instalando o pacote `Microsoft.IdentityModel.Tokens` e `System.IdentityModel.Tokens.Jwt`

### TokenService

- Criei a pasta Services e 1 classe TokenService, vai ser a responsavel por gerar o token
- No tokenService fez algo que faz como coletar o ID do funcionario durante o codigo pelo token
- Essa classe gera o token e return ele

```csharp
public class TokenService
{
    public static object GenerateToken(Employee employee)
    { 
        var key = Encoding.ASCII.GetBytes(Key.Secret);
        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim("employeeId", employee.id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);

        return new
        {
            token = tokenString
        };
    }

}
}
```

### Validação do token

- Um metodo no program para essa validação

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });
});
```

---

## Arquitetura Web API

![Arquitetura Web API](image/DIARIO/1790282820154.png)

---

## Auto Mapper

Baixei 2 extensão, as 2 primeiro que lista: AutoMapper e AutoMapperDepedencyInjection, para mapear a DTO automaticamente

> [!IMPORTANT]
> Importante iniciar o auto mapper no Program.cs, as classes dto ter os mesmo nomes que a classe

```csharp
var employees = _employeeRepository.Get(id);
var employeesDTO = _mapper.Map<EmployeeDTO>(employees);
```

Criado a pasta Mapping e criado o construtor para colocar a configração do mapper

---

## Migrations

Pegar a entidade(classe) e criar a tabela no banco de dados automaticamente pelo Entity, migrations

Configurar as classes/tabela via Dbcontext e ai depois cada mudança fazer Add-Migrations e dps update-database

---

## Complementos (revisão do Claude)

> [!TIP]
> Essa parte não fui eu que escrevi, são complementos que o Claude sugeriu olhando meu código. Deixei separado das minhas anotações.

### Migrations - comandos certinhos

No Console do Gerenciador de Pacotes do Visual Studio é no singular:

```powershell
Add-Migration NomeDaMudanca
Update-Database
```

Pelo terminal (precisa da ferramenta `dotnet-ef`):

```bash
dotnet ef migrations add NomeDaMudanca
dotnet ef database update
```

Já tenho 2 migrations na pasta `Migrations`: `InitialMigration` e `CompanyTable`. O `ConnectionContextModelSnapshot.cs` é tipo 1 "foto" de como o banco tá agora, o EF compara com ela pra saber o que mudou

### Auto Mapper - quando o nome é diferente

Se o nome da DTO for diferente da classe, não tem problema, é só falar pro mapper qual campo vai em qual com o `ForMember`:

```csharp
CreateMap<Employee, EmployeeDTO>()
    .ForMember(dest => dest.NameEmployee, m => m.MapFrom(orig => orig.name));
```

- Registro no Program: `builder.Services.AddAutoMapper(cfg => { }, typeof(DomainToDTOMapping));`
- Nas versões novas a injeção de dependência já vem dentro do pacote `AutoMapper`, então nem precisa daquele segundo pacote (meu .csproj só tem o AutoMapper mesmo)
- Da versão 15 pra frente o AutoMapper virou comercial e pede 1 chave de licença, tem 1 versão Community grátis. Bom confirmar isso antes de usar no trabalho

### Injeção de dependência - tempo de vida

Eu uso `AddTransient`, mas tem 3 tipos:

| Tipo | Quando cria o objeto |
|---|---|
| `Transient` | toda vez que alguém pede, sempre 1 novo |
| `Scoped` | 1 por requisição HTTP (padrão pra DbContext e repository) |
| `Singleton` | 1 só enquanto a API tá rodando |

### DbContext pela injeção (próximo passo)

Hoje o repository faz `new ConnectionContext()` na mão. O mais comum é deixar o .NET criar pra mim:

```csharp
builder.Services.AddDbContext<ConnectionContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
```

Aí o repository recebe o `ConnectionContext` pelo construtor, igual o controller já recebe o `IMapper`

### Segredos fora do código ⚠️

O `Key.Secret` do JWT e a senha do banco tão escritos direto no código e vão junto pro GitHub. O certo é jogar isso pro `appsettings.Development.json` ou melhor ainda usar o `dotnet user-secrets`, que guarda fora da pasta do projeto

### Coisas que já fiz e não tinha anotado

- **Versionamento da API**: pastas `Controllers/v1` e `v2`, com `[ApiVersion("1.0")]` e a rota `api/v{version:apiVersion}/employee`. O pacote `Microsoft.AspNetCore.Mvc.Versioning` foi descontinuado, o novo é o `Asp.Versioning.Mvc`
- **Paginação**: o Get usa `Skip(pageNumber * pageQuantity).Take(pageQuantity)`, exemplo bom de LINQ no lugar de SQL
- **Tratamento de erro**: o `ThrowController` tem as rotas `/error` e `/error-development`, usadas pelo `UseExceptionHandler`, e devolvem 1 `Problem()`
- **CORS**: a política `MyPolicy` libera o `localhost:8080` pra acessar a API
- **Proteger rota**: é o `[Authorize]` em cima do endpoint que faz ele pedir o token JWT

### 2 ajustes pequenos pra fazer

- A rota de download da foto tá como `[HttpPost]`, mas ela só busca, então o certo seria `[HttpGet]`
- Se o ID não existir o `employee.photo` estoura erro, colocar 1 `if (employee == null) return NotFound();` antes
