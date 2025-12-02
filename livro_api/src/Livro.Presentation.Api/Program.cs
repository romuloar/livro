using Livro.Presentation.Hosting;
using Livro.Presentation.Api.Extensions;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Seeds;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200", "http://localhost:4201")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Configuração do banco SQLite
// Usa /app/data para persistir dados entre rebuilds do container
var connectionString = "Data Source=/app/data/livro.db";
builder.Services.AddLivroServices(connectionString);

var app = builder.Build();

// Inicializa banco de dados (cria schema + executa seeds automaticamente)
await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirecionar raiz para Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

