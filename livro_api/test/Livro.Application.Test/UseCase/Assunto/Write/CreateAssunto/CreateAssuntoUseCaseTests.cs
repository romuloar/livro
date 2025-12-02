using Xunit;
using NSubstitute;
using FluentAssertions;
using Livro.Application.UseCase.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result.Extensions;

namespace Livro.Application.Test.UseCase.Assunto.Write.CreateAssunto;

public class CreateAssuntoUseCaseTests
{
    private readonly ICreateAssuntoPort _mockPort;
    private readonly CreateAssuntoUseCase _useCase;

    public CreateAssuntoUseCaseTests()
    {
        _mockPort = Substitute.For<ICreateAssuntoPort>();
        _useCase = new CreateAssuntoUseCase(_mockPort);
    }

    [Fact]
    public async Task ExecuteAsync_DescricaoValida_DeveCriarAssuntoComSucesso()
    {
        // Arrange
        var input = new CreateAssuntoIn { Descricao = "Ficção Científica" };
        var assuntoCriado = new AssuntoDomain { CodAs = Ulid.NewUlid(), Descricao = "Ficção Científica" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(assuntoCriado.GetResultDetailSuccess("Assunto criado"));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.ResultData.Should().NotBeNull();
        resultado.ResultData!.Descricao.Should().Be("Ficção Científica");
        
        await _mockPort.Received(1).ExecuteAsync(input);
    }

    [Fact]
    public async Task ExecuteAsync_DescricaoVazia_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var input = new CreateAssuntoIn { Descricao = "" };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("Parametros");
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateAssuntoIn>());
    }

    [Fact]
    public async Task ExecuteAsync_DescricaoMuitoLonga_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var descricaoLonga = new string('A', 50); // Maior que o limite de 20 caracteres
        var input = new CreateAssuntoIn { Descricao = descricaoLonga };

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().NotBeNullOrEmpty();
        
        await _mockPort.DidNotReceive().ExecuteAsync(Arg.Any<CreateAssuntoIn>());
    }

    [Fact]
    public async Task ExecuteAsync_PortRetornaErro_DeveRepassarErroDoPort()
    {
        // Arrange
        var input = new CreateAssuntoIn { Descricao = "Romance" };
        
        _mockPort.ExecuteAsync(input)
            .Returns(Task.FromResult(
                await ResultDetailExtensions.GetErrorAsync<AssuntoDomain>("Erro ao salvar no banco")
            ));

        // Act
        var resultado = await _useCase.ExecuteAsync(input);

        // Assert
        resultado.IsSuccess.Should().BeFalse();
        resultado.Message.Should().Contain("banco");
    }
}
