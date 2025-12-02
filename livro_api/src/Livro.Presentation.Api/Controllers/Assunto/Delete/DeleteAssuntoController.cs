using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Assunto.Delete;

[ApiController]
[Route(ApiRoutes.Assunto.Delete)]
[Tags("Assuntos")]
public class DeleteAssuntoController : ControllerBase
{
    private readonly IDeleteAssuntoUseCase _deleteAssuntoUseCase;

    public DeleteAssuntoController(IDeleteAssuntoUseCase deleteAssuntoUseCase)
    {
        _deleteAssuntoUseCase = deleteAssuntoUseCase;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Ulid id)
    {
        var result = await _deleteAssuntoUseCase.ExecuteAsync(new DeleteAssuntoIn { Id = id });
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message });
    }
}
