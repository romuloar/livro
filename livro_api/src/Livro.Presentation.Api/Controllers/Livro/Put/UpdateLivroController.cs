using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.UpdateLivro.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Livro.Put;

[ApiController]
[Route(ApiRoutes.Livro.Update)]
[Tags("Livros")]
public class UpdateLivroController : ControllerBase
{
    private readonly IUpdateLivroUseCase _updateLivroUseCase;

    public UpdateLivroController(IUpdateLivroUseCase updateLivroUseCase)
    {
        _updateLivroUseCase = updateLivroUseCase;
    }

    [HttpPut]
    public async Task<IActionResult> Update(Ulid id, [FromBody] UpdateLivroIn input)
    {
        input.Id = id;
        var result = await _updateLivroUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });
        
        return Ok(new { message = result.Message, data = result.ResultData });
    }
}
