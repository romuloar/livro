using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Domain.Entity.Livro;
using Rom.Result.Extensions;
using Livro.Domain.Port.Livro.Read.GetLivroById.Out;

namespace Livro.Application.Test.UseCase.Livro.Read.GetLivroById;

public class GetLivroByIdUseCaseTests
{
    private readonly IGetLivroByIdPort _mockPort;
    private readonly GetLivroByIdUseCase _useCase;

    public GetLivroByIdUseCaseTests()
    {
        _mockPort = Substitute.For<IGetLivroByIdPort>();
        _useCase = new GetLivroByIdUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_IdValido_DeveRetornarLivro()
    {
        // Arrange
        var testId = Ulid.NewUlid();
        var input = new GetLivroByIdIn { Id = testId };
        var livroEsperado = new LivroDomain
        {
            Codl = testId,
            Titulo = "Dom Casmurro",
            Editora = "Record",
            Edicao = 1,
            AnoPublicacao = "1899"
        };

        var outEsperado = new GetLivroByIdOut
        {
            Codl = livroEsperado.Codl,
            Titulo = livroEsperado.Titulo,
            Editora = livroEsperado.Editora,
            Edicao = livroEsperado.Edicao,
            AnoPublicacao = livroEsperado.AnoPublicacao
        };

        _mockPort.ExecuteAsync(input)
            .Returns(outEsperado.GetResultDetailSuccess("Livro encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().NotBeNull();
        resultado.ResultData.Titulo.Should().Be("Dom Casmurro");
        resultado.ResultData.Codl.Should().Be(testId);

        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_LivroNaoEncontrado_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new GetLivroByIdIn { Id = Ulid.NewUlid() };

        _mockPort.ExecuteAsync(input)
            .Returns(await ResultDetailExtensions.GetErrorAsync<GetLivroByIdOut>("Livro não encontrado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
        resultado.ResultData.Should().BeNull();
    }
}
