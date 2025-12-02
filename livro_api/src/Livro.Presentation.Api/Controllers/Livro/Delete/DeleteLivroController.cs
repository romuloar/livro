using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Livro.Delete;

[ApiController]
[Route(ApiRoutes.Livro.Delete)]
[Tags("Livros")]
public class DeleteLivroController : ControllerBase
{
    private readonly IDeleteLivroUseCase _deleteLivroUseCase;

    public DeleteLivroController(IDeleteLivroUseCase deleteLivroUseCase)
    {
        _deleteLivroUseCase = deleteLivroUseCase;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Ulid id)
    {
        var result = await _deleteLivroUseCase.ExecuteAsync(new DeleteLivroIn { Id = id });
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message });
    }
}
