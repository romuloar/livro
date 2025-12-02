using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Assunto.Write.UpdateAssunto;

public class UpdateAssuntoUseCaseTests
{
    private readonly IUpdateAssuntoPort _mockPort;
    private readonly UpdateAssuntoUseCase _useCase;

    public UpdateAssuntoUseCaseTests()
    {
        _mockPort = Substitute.For<IUpdateAssuntoPort>();
        _useCase = new UpdateAssuntoUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_DadosValidos_DeveAtualizarAssunto()
    {
        // Arrange
        var testId = Ulid.NewUlid();
        var input = new UpdateAssuntoIn { Id = testId, Descricao = "Terror Psicológico" };
        var assuntoAtualizado = new AssuntoDomain { CodAs = testId, Descricao = "Terror Psicológico" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(assuntoAtualizado.GetResultDetailSuccess("Atualizado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData!.CodAs.Should().Be(testId);
        resultado.ResultData.Descricao.Should().Be("Terror Psicológico");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_DescricaoVazia_DeveRetornarErro()
    {
        // Arrange
        var input = new UpdateAssuntoIn { Id = Ulid.NewUlid(), Descricao = "" };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<UpdateAssuntoIn>());
    }

    [Fact]
    public async Task ExecuteAsync_AssuntoNaoEncontrado_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new UpdateAssuntoIn { Id = Ulid.NewUlid(), Descricao = "Aventura" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Assunto não encontrado")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
