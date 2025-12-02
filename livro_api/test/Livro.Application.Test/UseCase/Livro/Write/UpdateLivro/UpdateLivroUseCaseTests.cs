using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Livro.Write.UpdateLivro;

public class UpdateLivroUseCaseTests
{
    private readonly IUpdateLivroPort _mockPort;
    private readonly UpdateLivroUseCase _useCase;

    public UpdateLivroUseCaseTests()
    {
        _mockPort = Substitute.For<IUpdateLivroPort>();
        _useCase = new UpdateLivroUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_DadosValidos_DeveAtualizarLivro()
    {
        // Arrange
        var testId = Ulid.NewUlid();
        var autorId = Ulid.NewUlid();
        var assuntoId1 = Ulid.NewUlid();
        var assuntoId2 = Ulid.NewUlid();
        var tipoCompraId = Ulid.NewUlid();
        
        var input = new UpdateLivroIn
        {
            Id = testId,
            Titulo = "Dom Casmurro - Edição Revisada",
            Editora = "Companhia das Letras",
            Edicao = 2,
            AnoPublicacao = "2020",
            AutoresIds = new List<Ulid> { autorId },
            AssuntosIds = new List<Ulid> { assuntoId1, assuntoId2 },
            Valores = new List<LivroValorUpdateIn>
            {
                new LivroValorUpdateIn { TipoCompraId = tipoCompraId, Valor = 55.00m }
            }
        };
        
        var livroAtualizado = new LivroDomain
        {
            Codl = testId,
            Titulo = "Dom Casmurro - Edição Revisada",
            Editora = "Companhia das Letras",
            Edicao = 2,
            AnoPublicacao = "2020"
        };
        
        _mockPort.ExecuteAsync(input)
            .Returns(livroAtualizado.GetResultDetailSuccess("Livro atualizado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Titulo.Should().Be("Dom Casmurro - Edição Revisada");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_SemAssuntos_DeveRetornarErro()
    {
        // Arrange
        var input = new UpdateLivroIn
        {
            Id = Ulid.NewUlid(),
            Titulo = "Esaú e Jacó",
            Editora = "Ática",
            Edicao = 1,
            AnoPublicacao = "1904",
            AutoresIds = new List<Ulid> { Ulid.NewUlid() },
            AssuntosIds = new List<Ulid>(), // Lista vazia
            Valores = new List<LivroValorUpdateIn>
            {
                new LivroValorUpdateIn { TipoCompraId = Ulid.NewUlid(), Valor = 35.00m }
            }
        };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<UpdateLivroIn>());
    }

    [Fact]
    public async Task ExecuteAsync_LivroNaoEncontrado_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new UpdateLivroIn
        {
            Id = Ulid.NewUlid(),
            Titulo = "Memorial de Aires",
            Editora = "Globo",
            Edicao = 1,
            AnoPublicacao = "1908",
            AutoresIds = new List<Ulid> { Ulid.NewUlid() },
            AssuntosIds = new List<Ulid> { Ulid.NewUlid() },
            Valores = new List<LivroValorUpdateIn>
            {
                new LivroValorUpdateIn { TipoCompraId = Ulid.NewUlid(), Valor = 42.00m }
            }
        };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<LivroDomain>("Livro não encontrado")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
