using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Autor.Delete;

[ApiController]
[Route(ApiRoutes.Autor.Delete)]
[Tags("Autores")]
public class DeleteAutorController : ControllerBase
{
    private readonly IDeleteAutorUseCase _deleteAutorUseCase;

    public DeleteAutorController(IDeleteAutorUseCase deleteAutorUseCase)
    {
        _deleteAutorUseCase = deleteAutorUseCase;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Ulid id)
    {
        var result = await _deleteAutorUseCase.ExecuteAsync(new DeleteAutorIn { Id = id });
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message });
    }
}
