using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Write.CreateAutor.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Autor.Post;

[ApiController]
[Route(ApiRoutes.Autor.Create)]
[Tags("Autores")]
public class CreateAutorController : ControllerBase
{
    private readonly ICreateAutorUseCase _createAutorUseCase;

    public CreateAutorController(ICreateAutorUseCase createAutorUseCase)
    {
        _createAutorUseCase = createAutorUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAutorIn input)
    {
        var result = await _createAutorUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Created($"{ApiRoutes.Autor.GetAll}/{result.ResultData?.CodAu}", result);
    }
}
