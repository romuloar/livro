using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Read.GetLivroById.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Livro.Get;

[ApiController]
[Route(ApiRoutes.Livro.GetById)]
[Tags("Livros")]
public class GetLivroByIdController : ControllerBase
{
    private readonly IGetLivroByIdUseCase _getLivroByIdUseCase;

    public GetLivroByIdController(IGetLivroByIdUseCase getLivroByIdUseCase)
    {
        _getLivroByIdUseCase = getLivroByIdUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out var ulidId))
            return BadRequest(new { errors = new { id = new[] { "The input was not valid." } } });
            
        var result = await _getLivroByIdUseCase.ExecuteAsync(new GetLivroByIdIn { Id = ulidId });
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
