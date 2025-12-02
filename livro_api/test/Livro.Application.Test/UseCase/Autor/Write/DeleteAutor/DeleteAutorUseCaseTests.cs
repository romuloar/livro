using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Autor.Write.DeleteAutor;

public class DeleteAutorUseCaseTests
{
    private readonly IDeleteAutorPort _mockPort;
    private readonly DeleteAutorUseCase _useCase;

    public DeleteAutorUseCaseTests()
    {
        _mockPort = Substitute.For<IDeleteAutorPort>();
        _useCase = new DeleteAutorUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_IdValido_DeveExcluirComSucesso()
    {
        // Arrange
        var input = new DeleteAutorIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(true.GetResultDetailSuccess("Autor excluído"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().BeTrue();
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_AutorNaoExiste_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new DeleteAutorIn { Id = Ulid.NewUlid() };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<bool>("Autor não encontrado")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
