using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Assunto.Put;

[ApiController]
[Route(ApiRoutes.Assunto.Update)]
[Tags("Assuntos")]
public class UpdateAssuntoController : ControllerBase
{
    private readonly IUpdateAssuntoUseCase _updateAssuntoUseCase;

    public UpdateAssuntoController(IUpdateAssuntoUseCase updateAssuntoUseCase)
    {
        _updateAssuntoUseCase = updateAssuntoUseCase;
    }

    [HttpPut]
    public async Task<IActionResult> Update(Ulid id, [FromBody] UpdateAssuntoIn input)
    {
        input.Id = id;
        var result = await _updateAssuntoUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message, data = result.ResultData });
    }
}
