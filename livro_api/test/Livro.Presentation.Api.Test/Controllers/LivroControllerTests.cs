using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Livro.Presentation.Api.Test.Controllers;

public class LivroControllerTests 
    //: IClassFixture<WebApplicationFactory<Program>>
{
    //private readonly HttpClient _client;

    //public LivroControllerTests(WebApplicationFactory<Program> factory)
    //{
    //    _client = factory.CreateClient();
    //}

    //[Fact]
    //public async Task GetLivros_ShouldReturnOk()
    //{
    //    // Act
    //    var response = await _client.GetAsync("/api/livro");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //}

    //[Fact]
    //public async Task GetLivros_ShouldReturnJsonArray()
    //{
    //    // Act
    //    var livros = await _client.GetFromJsonAsync<object[]>("/api/livro");

    //    // Assert
    //    livros.Should().NotBeNull();
    //    livros.Should().BeOfType<object[]>();
    //}
}
