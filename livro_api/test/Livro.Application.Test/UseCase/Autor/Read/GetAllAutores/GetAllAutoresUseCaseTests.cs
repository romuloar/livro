using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Autor.Read.GetAllAutores;
using Livro.Domain.Port.Autor.Read.GetAllAutores;
using Livro.Domain.Entity.Autor;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Autor.Read.GetAllAutores;

public class GetAllAutoresUseCaseTests
{
    private readonly IGetAllAutoresPort _mockPort;
    private readonly GetAllAutoresUseCase _useCase;

    public GetAllAutoresUseCaseTests()
    {
        _mockPort = Substitute.For<IGetAllAutoresPort>();
        _useCase = new GetAllAutoresUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_ListaComAutores_DeveRetornarTodos()
    {
        // Arrange
        var autores = new List<AutorDomain>
        {
            new AutorDomain { CodAu = Ulid.NewUlid(), Nome = "Machado de Assis" },
            new AutorDomain { CodAu = Ulid.NewUlid(), Nome = "Jorge Amado" },
            new AutorDomain { CodAu = Ulid.NewUlid(), Nome = "Clarice Lispector" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(autores.GetResultDetailSuccess("Autores recuperados"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(3);
        resultado.ResultData.Should().ContainSingle(a => a.Nome == "Machado de Assis");
        resultado.ResultData.Should().ContainSingle(a => a.Nome == "Jorge Amado");
        resultado.ResultData.Should().ContainSingle(a => a.Nome == "Clarice Lispector");
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ListaVazia_DeveRetornarListaVaziaMasValida()
    {
        // Arrange
        var listaVazia = new List<AutorDomain>();
        
        _mockPort.ExecuteAsync()
            .Returns(listaVazia.GetResultDetailSuccess("Nenhum autor cadastrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().BeEmpty();
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ErroNoBancoDeDados_DeveRetornarErroDoPort()
    {
        // Arrange
        _mockPort.ExecuteAsync()
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<List<AutorDomain>>("Erro de conexão")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("conexão");
        resultado.ResultData.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ListaComUmAutor_DeveRetornarApenaUm()
    {
        // Arrange
        var autores = new List<AutorDomain>
        {
            new AutorDomain { CodAu = Ulid.NewUlid(), Nome = "Cecília Meireles" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(autores.GetResultDetailSuccess("Um autor encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(1);
        resultado.ResultData.First().Nome.Should().Be("Cecília Meireles");
    }
}
