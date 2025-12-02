using FluentAssertions;
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Livro.Infra.EfCore.Test.Adapters;

/// <summary>
/// Exemplo de teste com EF Core InMemory Database
/// 
/// Padrão:
/// - IDisposable para limpar o banco após cada teste
/// - UseInMemoryDatabase com GUID único para isolamento
/// - FluentAssertions para verificações expressivas
/// </summary>
public class LivroEntityTests : IDisposable
{
    private readonly AppDbContext _context;

    public LivroEntityTests()
    {
        // Configura banco InMemory para testes (cada teste tem seu próprio banco)
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
    }

    [Fact]
    public async Task AddLivro_WhenValid_ShouldSaveToDatabase()
    {
        // Arrange
        var livro = new LivroEntity 
        { 
            Titulo = "Clean Code", 
            Editora = "Prentice Hall", 
            Edicao = 1, 
            AnoPublicacao = "2008" 
        };

        // Act
        await _context.Livros.AddAsync(livro);
        await _context.SaveChangesAsync();
        
        var result = await _context.Livros.FirstOrDefaultAsync(l => l.Titulo == "Clean Code");

        // Assert
        result.Should().NotBeNull();
        result!.Titulo.Should().Be("Clean Code");
        result.Editora.Should().Be("Prentice Hall");
        result.Edicao.Should().Be(1);
    }

    [Fact]
    public async Task GetLivros_WhenMultipleExist_ShouldReturnAll()
    {
        // Arrange
        var livros = new[]
        {
            new LivroEntity { Titulo = "Book 1", Editora = "Publisher 1", Edicao = 1, AnoPublicacao = "2020" },
            new LivroEntity { Titulo = "Book 2", Editora = "Publisher 2", Edicao = 1, AnoPublicacao = "2021" }
        };
        await _context.Livros.AddRangeAsync(livros);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.Livros.ToListAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(l => l.Titulo == "Book 1");
        result.Should().Contain(l => l.Titulo == "Book 2");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
