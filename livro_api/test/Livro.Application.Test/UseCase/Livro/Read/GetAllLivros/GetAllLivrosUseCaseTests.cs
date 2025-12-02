using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Livro.Read.GetAllLivros;
using Livro.Domain.Port.Livro.Read.GetAllLivros;
using Livro.Domain.Entity.Livro;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Livro.Read.GetAllLivros;

public class GetAllLivrosUseCaseTests
{
    private readonly IGetAllLivrosPort _mockPort;
    private readonly GetAllLivrosUseCase _useCase;

    public GetAllLivrosUseCaseTests()
    {
        _mockPort = Substitute.For<IGetAllLivrosPort>();
        _useCase = new GetAllLivrosUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_ListaComLivros_DeveRetornarTodos()
    {
        // Arrange
        var livros = new List<LivroDomain>
        {
            new LivroDomain { Codl = Ulid.NewUlid(), Titulo = "Dom Casmurro", Editora = "Record", Edicao = 1, AnoPublicacao = "1899" },
            new LivroDomain { Codl = Ulid.NewUlid(), Titulo = "Quincas Borba", Editora = "Saraiva", Edicao = 1, AnoPublicacao = "1891" },
            new LivroDomain { Codl = Ulid.NewUlid(), Titulo = "Memórias Póstumas", Editora = "Nova Fronteira", Edicao = 2, AnoPublicacao = "1881" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(livros.GetResultDetailSuccess("Livros recuperados"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(3);
        resultado.ResultData.Should().ContainSingle(l => l.Titulo == "Dom Casmurro");
        resultado.ResultData.Should().ContainSingle(l => l.Titulo == "Quincas Borba");
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ListaVazia_DeveRetornarListaVaziaMasValida()
    {
        // Arrange
        var listaVazia = new List<LivroDomain>();
        
        _mockPort.ExecuteAsync()
            .Returns(listaVazia.GetResultDetailSuccess("Nenhum livro cadastrado"));

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
                await ResultDetailExtensions.GetErrorAsync<List<LivroDomain>>("Erro de conexão")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("conexão");
        resultado.ResultData.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ListaComUmLivro_DeveRetornarApenaUm()
    {
        // Arrange
        var livros = new List<LivroDomain>
        {
            new LivroDomain { Codl = Ulid.NewUlid(), Titulo = "Esaú e Jacó", Editora = "Ática", Edicao = 1, AnoPublicacao = "1904" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(livros.GetResultDetailSuccess("Um livro encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(1);
        resultado.ResultData.First().Titulo.Should().Be("Esaú e Jacó");
    }
}
