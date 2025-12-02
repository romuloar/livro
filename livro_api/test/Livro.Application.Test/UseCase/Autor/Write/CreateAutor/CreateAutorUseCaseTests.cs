using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Domain.Entity.Autor;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Autor.Write.CreateAutor;

public class CreateAutorUseCaseTests
{
    private readonly ICreateAutorPort _mockPort;
    private readonly CreateAutorUseCase _useCase;

    public CreateAutorUseCaseTests()
    {
        _mockPort = Substitute.For<ICreateAutorPort>();
        _useCase = new CreateAutorUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_NomeValido_DeveCriarAutorComSucesso()
    {
        // Arrange
        var input = new CreateAutorIn { Nome = "Machado de Assis" };
        var autorEsperado = new AutorDomain { CodAu = Ulid.NewUlid(), Nome = "Machado de Assis" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(autorEsperado.GetResultDetailSuccess("Autor criado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().NotBeNull();
        resultado.ResultData.Nome.Should().Be("Machado de Assis");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_NomeVazio_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var input = new CreateAutorIn { Nome = "" };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateAutorIn>());
    }

    [Fact]
    public async Task ExecuteAsync_NomeMuitoLongo_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var input = new CreateAutorIn { Nome = new string('A', 41) }; // 41 caracteres (limite é 40)

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("inválidos");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateAutorIn>());
    }

    [Fact]
    public async Task ExecuteAsync_PortRetornaErro_DeveRepassarErroDoPort()
    {
        // Arrange
        var input = new CreateAutorIn { Nome = "Jorge Amado" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<AutorDomain>("Erro ao salvar no banco")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("banco");
    }
}
