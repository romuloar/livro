using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Livro.Write.DeleteLivro;

public class DeleteLivroUseCaseTests
{
    private readonly IDeleteLivroPort _mockPort;
    private readonly DeleteLivroUseCase _useCase;

    public DeleteLivroUseCaseTests()
    {
        _mockPort = Substitute.For<IDeleteLivroPort>();
        _useCase = new DeleteLivroUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_IdValido_DeveExcluirComSucesso()
    {
        // Arrange
        var input = new DeleteLivroIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(true.GetResultDetailSuccess("Livro excluído"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().BeTrue();
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_LivroNaoExiste_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new DeleteLivroIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<bool>("Livro não encontrado")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
