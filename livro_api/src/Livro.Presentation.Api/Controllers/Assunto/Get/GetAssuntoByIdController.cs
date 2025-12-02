using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Assunto.Read.GetAssuntoById;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;
using Livro.Presentation.Api.Constants;

namespace Livro.Presentation.Api.Controllers.Assunto.Get;

[ApiController]
[Route(ApiRoutes.Assunto.GetById)]
[Tags("Assuntos")]
public class GetAssuntoByIdController : ControllerBase
{
    private readonly IGetAssuntoByIdUseCase _getAssuntoByIdUseCase;

    public GetAssuntoByIdController(IGetAssuntoByIdUseCase getAssuntoByIdUseCase)
    {
        _getAssuntoByIdUseCase = getAssuntoByIdUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out var ulidId))
            return BadRequest(new { errors = new { id = new[] { "The input was not valid." } } });
            
        var result = await _getAssuntoByIdUseCase.ExecuteAsync(new GetAssuntoByIdIn { Id = ulidId });
        
        if (!result.IsSuccess)
            return NotFound(result);
        
        return Ok(result);
    }
}
