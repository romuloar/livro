using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Assunto.Read.GetAllAssuntos;
using Livro.Domain.Port.Assunto.Read.GetAllAssuntos;
using Livro.Domain.Entity.Assunto;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Assunto.Read.GetAllAssuntos;

public class GetAllAssuntosUseCaseTests
{
    private readonly IGetAllAssuntosPort _mockPort;
    private readonly GetAllAssuntosUseCase _useCase;

    public GetAllAssuntosUseCaseTests()
    {
        _mockPort = Substitute.For<IGetAllAssuntosPort>();
        _useCase = new GetAllAssuntosUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_ListaComAssuntos_DeveRetornarTodos()
    {
        // Arrange
        var assuntos = new List<AssuntoDomain>
        {
            new AssuntoDomain { CodAs = Ulid.NewUlid(), Descricao = "Ficção" },
            new AssuntoDomain { CodAs = Ulid.NewUlid(), Descricao = "Drama" },
            new AssuntoDomain { CodAs = Ulid.NewUlid(), Descricao = "Suspense" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(assuntos.GetResultDetailSuccess("Lista recuperada"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(3);
        resultado.ResultData.Should().ContainSingle(a => a.Descricao == "Ficção");
        resultado.ResultData.Should().ContainSingle(a => a.Descricao == "Drama");
        resultado.ResultData.Should().ContainSingle(a => a.Descricao == "Suspense");
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ListaVazia_DeveRetornarListaVaziaMasValida()
    {
        // Arrange
        var listaVazia = new List<AssuntoDomain>();
        
        _mockPort.ExecuteAsync()
            .Returns(listaVazia.GetResultDetailSuccess("Nenhum assunto cadastrado"));

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
                await ResultDetailExtensions.GetErrorAsync<List<AssuntoDomain>>("Erro de conexão com banco")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("conexão");
        resultado.ResultData.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ListaComUmAssunto_DeveRetornarApenaUm()
    {
        // Arrange
        var assuntos = new List<AssuntoDomain>
        {
            new AssuntoDomain { CodAs = Ulid.NewUlid(), Descricao = "Biografia" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(assuntos.GetResultDetailSuccess("Um assunto encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(1);
        resultado.ResultData.First().Descricao.Should().Be("Biografia");
    }
}
