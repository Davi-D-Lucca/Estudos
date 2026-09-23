using PrimeiraApi.Infraestrutura;
using PrimeiraApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o suporte aos Controllers e ao Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Habilita o Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Ativa a rota /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();