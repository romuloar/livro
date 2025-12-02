using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.UpdateAutor.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Autor.Put;

[ApiController]
[Route(ApiRoutes.Autor.Update)]
[Tags("Autores")]
public class UpdateAutorController : ControllerBase
{
    private readonly IUpdateAutorUseCase _updateAutorUseCase;

    public UpdateAutorController(IUpdateAutorUseCase updateAutorUseCase)
    {
        _updateAutorUseCase = updateAutorUseCase;
    }

    [HttpPut]
    public async Task<IActionResult> Update(Ulid id, [FromBody] UpdateAutorIn input)
    {
        input.Id = id;
        var result = await _updateAutorUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message, data = result.ResultData });
    }
}
