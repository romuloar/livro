using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Write.CreateLivro.In;
using Livro.Domain.Entity.Livro;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Livro.Write.CreateLivro;

public class CreateLivroUseCaseTests
{
    private readonly ICreateLivroPort _mockPort;
    private readonly CreateLivroUseCase _useCase;

    public CreateLivroUseCaseTests()
    {
        _mockPort = Substitute.For<ICreateLivroPort>();
        _useCase = new CreateLivroUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_DadosValidos_DeveCriarLivroComSucesso()
    {
        // Arrange
        var testId = Ulid.NewUlid();
        var autorId = Ulid.NewUlid();
        var assuntoId1 = Ulid.NewUlid();
        var assuntoId2 = Ulid.NewUlid();
        var tipoCompraId = Ulid.NewUlid();
        
        var input = new CreateLivroIn
        {
            Titulo = "Dom Casmurro",
            Editora = "Editora Record",
            Edicao = 1,
            AnoPublicacao = "1899",
            AutoresIds = new List<Ulid> { autorId },
            AssuntosIds = new List<Ulid> { assuntoId1, assuntoId2 },
            Valores = new List<LivroValorIn>
            {
                new LivroValorIn { TipoCompraId = tipoCompraId, Valor = 45.90m }
            }
        };
        
        var livroEsperado = new LivroDomain
        {
            Codl = testId,
            Titulo = "Dom Casmurro",
            Editora = "Editora Record",
            Edicao = 1,
            AnoPublicacao = "1899"
        };
        
        _mockPort.ExecuteAsync(input)
            .Returns(livroEsperado.GetResultDetailSuccess("Livro criado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().NotBeNull();
        resultado.ResultData.Titulo.Should().Be("Dom Casmurro");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_TituloVazio_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var input = new CreateLivroIn
        {
            Titulo = "",
            Editora = "Editora Record",
            Edicao = 1,
            AnoPublicacao = "1899",
            AutoresIds = new List<Ulid> { Ulid.NewUlid() },
            AssuntosIds = new List<Ulid> { Ulid.NewUlid() },
            Valores = new List<LivroValorIn>
            {
                new LivroValorIn { TipoCompraId = Ulid.NewUlid(), Valor = 45.90m }
            }
        };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateLivroIn>());
    }

    [Fact]
    public async Task ExecuteAsync_SemAutores_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var input = new CreateLivroIn
        {
            Titulo = "Memórias Póstumas",
            Editora = "Nova Fronteira",
            Edicao = 1,
            AnoPublicacao = "1881",
            AutoresIds = new List<Ulid>(), // Lista vazia
            AssuntosIds = new List<Ulid> { Ulid.NewUlid() },
            Valores = new List<LivroValorIn>
            {
                new LivroValorIn { TipoCompraId = Ulid.NewUlid(), Valor = 39.90m }
            }
        };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateLivroIn>());
    }

    [Fact]
    public async Task ExecuteAsync_PortRetornaErro_DeveRepassarErroDoPort()
    {
        // Arrange
        var input = new CreateLivroIn
        {
            Titulo = "Quincas Borba",
            Editora = "Saraiva",
            Edicao = 2,
            AnoPublicacao = "1891",
            AutoresIds = new List<Ulid> { Ulid.NewUlid() },
            AssuntosIds = new List<Ulid> { Ulid.NewUlid() },
            Valores = new List<LivroValorIn>
            {
                new LivroValorIn { TipoCompraId = Ulid.NewUlid(), Valor = 50.00m }
            }
        };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<LivroDomain>("Erro ao salvar no banco")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("banco");
    }
}
