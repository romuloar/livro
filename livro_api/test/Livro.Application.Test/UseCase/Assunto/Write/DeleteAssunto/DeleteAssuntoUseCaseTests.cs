using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Assunto.Write.DeleteAssunto;

public class DeleteAssuntoUseCaseTests
{
    private readonly IDeleteAssuntoPort _mockPort;
    private readonly DeleteAssuntoUseCase _useCase;

    public DeleteAssuntoUseCaseTests()
    {
        _mockPort = Substitute.For<IDeleteAssuntoPort>();
        _useCase = new DeleteAssuntoUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_IdValido_DeveExcluirComSucesso()
    {
        // Arrange
        var input = new DeleteAssuntoIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(true.GetResultDetailSuccess("Excluído"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().BeTrue();
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_AssuntoNaoExiste_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new DeleteAssuntoIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<bool>("Assunto não encontrado para exclusão")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
