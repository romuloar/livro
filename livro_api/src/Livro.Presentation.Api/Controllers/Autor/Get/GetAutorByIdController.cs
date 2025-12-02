using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Autor.Read.GetAutorById;
using Livro.Domain.Port.Autor.Read.GetAutorById.In;
using Livro.Presentation.Api.Constants;

namespace Livro.Presentation.Api.Controllers.Autor.Get;

[ApiController]
[Route(ApiRoutes.Autor.GetById)]
[Tags("Autores")]
public class GetAutorByIdController : ControllerBase
{
    private readonly IGetAutorByIdUseCase _getAutorByIdUseCase;

    public GetAutorByIdController(IGetAutorByIdUseCase getAutorByIdUseCase)
    {
        _getAutorByIdUseCase = getAutorByIdUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out var ulidId))
            return BadRequest(new { errors = new { id = new[] { "The input was not valid." } } });
            
        var result = await _getAutorByIdUseCase.ExecuteAsync(new GetAutorByIdIn { Id = ulidId });
        
        if (!result.IsSuccess)
            return NotFound(result);
        
        return Ok(result);
    }
}
