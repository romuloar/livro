using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Write.CreateLivro.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Livro.Post;

[ApiController]
[Route(ApiRoutes.Livro.Create)]
[Tags("Livros")]
public class CreateLivroController : ControllerBase
{
    private readonly ICreateLivroUseCase _createLivroUseCase;

    public CreateLivroController(ICreateLivroUseCase createLivroUseCase)
    {
        _createLivroUseCase = createLivroUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLivroIn input)
    {
        var result = await _createLivroUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Created($"{ApiRoutes.Livro.GetAll}/{result.ResultData?.Codl}", result);
    }
}
