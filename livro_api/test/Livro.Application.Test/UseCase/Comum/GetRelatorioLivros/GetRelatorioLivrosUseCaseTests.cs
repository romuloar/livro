using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Comum;
using Livro.Domain.Port.Relatorio.GetRelatorioLivros;
using Livro.Domain.Entity.Relatorio;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Comum.GetRelatorioLivros;

public class GetRelatorioLivrosUseCaseTests
{
    private readonly IGetRelatorioLivrosPort _mockPort;
    private readonly GetRelatorioLivrosUseCase _useCase;

    public GetRelatorioLivrosUseCaseTests()
    {
        _mockPort = Substitute.For<IGetRelatorioLivrosPort>();
        _useCase = new GetRelatorioLivrosUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_ListaComRelatorios_DeveRetornarTodos()
    {
        // Arrange
        var relatorios = new List<RelatorioLivroDomain>
        {
            new RelatorioLivroDomain
            {
                AutorNome = "Machado de Assis",
                LivroTitulo = "Dom Casmurro",
                Editora = "Record",
                Edicao = 1,
                AnoPublicacao = "1899",
                Assuntos = new List<string> { "Ficção", "Romance" },
                Valores = new List<ValorLivroDomain>
                {
                    new ValorLivroDomain { TipoCompra = "Balcão", Valor = 45.90m }
                }
            },
            new RelatorioLivroDomain
            {
                AutorNome = "Jorge Amado",
                LivroTitulo = "Capitães da Areia",
                Editora = "Companhia das Letras",
                Edicao = 2,
                AnoPublicacao = "1937",
                Assuntos = new List<string> { "Drama" },
                Valores = new List<ValorLivroDomain>
                {
                    new ValorLivroDomain { TipoCompra = "Internet", Valor = 39.90m }
                }
            }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(relatorios.GetResultDetailSuccess("Relatório gerado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(2);
        resultado.ResultData.Should().ContainSingle(r => r.LivroTitulo == "Dom Casmurro");
        resultado.ResultData.Should().ContainSingle(r => r.AutorNome == "Jorge Amado");
        
        await _mockPort.Received(1).ExecuteAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ListaVazia_DeveRetornarListaVaziaMasValida()
    {
        // Arrange
        var listaVazia = new List<RelatorioLivroDomain>();
        
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
                await ResultDetailExtensions.GetErrorAsync<List<RelatorioLivroDomain>>("Erro ao gerar relatório")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("relatório");
        resultado.ResultData.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ListaComUmRelatorio_DeveRetornarApenaUm()
    {
        // Arrange
        var relatorios = new List<RelatorioLivroDomain>
        {
            new RelatorioLivroDomain
            {
                AutorNome = "Clarice Lispector",
                LivroTitulo = "A Hora da Estrela",
                Editora = "Rocco",
                Edicao = 1,
                AnoPublicacao = "1977",
                Assuntos = new List<string> { "Ficção" },
                Valores = new List<ValorLivroDomain>
                {
                    new ValorLivroDomain { TipoCompra = "Self-Service", Valor = 32.50m }
                }
            }
        };
        
        _mockPort.ExecuteAsync()
            .Returns(relatorios.GetResultDetailSuccess("Um livro encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().HaveCount(1);
        resultado.ResultData.First().LivroTitulo.Should().Be("A Hora da Estrela");
        resultado.ResultData.First().AutorNome.Should().Be("Clarice Lispector");
    }
}
