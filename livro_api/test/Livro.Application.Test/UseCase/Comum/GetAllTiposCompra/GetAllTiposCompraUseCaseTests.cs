using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Comum;
using Livro.Domain.Port.TipoCompra.GetAllTiposCompra;
using Livro.Domain.Entity.TipoCompra;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Comum.GetAllTiposCompra;

public class GetAllTiposCompraUseCaseTests
{
    private readonly IGetAllTiposCompraPort _mockPort;
    private readonly GetAllTiposCompraUseCase _useCase;

    public GetAllTiposCompraUseCaseTests()
    {
        _mockPort = Substitute.For<IGetAllTiposCompraPort>();
        _useCase = new GetAllTiposCompraUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_ListaComTiposCompra_DeveRetornarTodos()
    {
        // Arrange
        var tipos = new List<TipoCompraDomain>
        {
            new TipoCompraDomain { CodTc = Ulid.NewUlid(), Descricao = "Balcão" },
            new TipoCompraDomain { CodTc = Ulid.NewUlid(), Descricao = "Self-Service" },
            new TipoCompraDomain { CodTc = Ulid.NewUlid(), Descricao = "Internet" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(tipos.GetResultDetailSuccess("Tipos de compra recuperados"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(3);
        resultado.ResultData.Should().ContainSingle(t => t.Descricao == "Balcão");
        resultado.ResultData.Should().ContainSingle(t => t.Descricao == "Internet");
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ListaVazia_DeveRetornarListaVaziaMasValida()
    {
        // Arrange
        var listaVazia = new List<TipoCompraDomain>();
        
        _mockPort.ExecuteAsync()
            .Returns(listaVazia.GetResultDetailSuccess("Nenhum tipo cadastrado"));

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
                await ResultDetailExtensions.GetErrorAsync<List<TipoCompraDomain>>("Erro de conexão")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("conexão");
        resultado.ResultData.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ListaComUmTipoCompra_DeveRetornarApenaUm()
    {
        // Arrange
        var tipos = new List<TipoCompraDomain>
        {
            new TipoCompraDomain { CodTc = Ulid.NewUlid(), Descricao = "Balcão" }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(tipos.GetResultDetailSuccess("Um tipo encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(1);
        resultado.ResultData.First().Descricao.Should().Be("Balcão");
    }
}
