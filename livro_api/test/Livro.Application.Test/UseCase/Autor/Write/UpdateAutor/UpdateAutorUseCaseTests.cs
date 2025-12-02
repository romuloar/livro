using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.UpdateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Autor.Write.UpdateAutor;

public class UpdateAutorUseCaseTests
{
    private readonly IUpdateAutorPort _mockPort;
    private readonly UpdateAutorUseCase _useCase;

    public UpdateAutorUseCaseTests()
    {
        _mockPort = Substitute.For<IUpdateAutorPort>();
        _useCase = new UpdateAutorUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_DadosValidos_DeveAtualizarAutor()
    {
        // Arrange
        var testId = Ulid.NewUlid();
        var input = new UpdateAutorIn { Id = testId, Nome = "Carlos Drummond de Andrade" };
        var autorAtualizado = new AutorDomain { CodAu = testId, Nome = "Carlos Drummond de Andrade" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(autorAtualizado.GetResultDetailSuccess("Autor atualizado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData!.CodAu.Should().Be(testId);
        resultado.ResultData.Nome.Should().Be("Carlos Drummond de Andrade");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_NomeVazio_DeveRetornarErro()
    {
        // Arrange
        var input = new UpdateAutorIn { Id = Ulid.NewUlid(), Nome = "" };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<UpdateAutorIn>());
    }

    [Fact]
    public async Task ExecuteAsync_AutorNaoEncontrado_DeveRetornarErroDoPort()
    {
        // Arrange
        var input = new UpdateAutorIn { Id = Ulid.NewUlid(), Nome = "Autor Inexistente" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Autor não encontrado")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("não encontrado");
    }
}
