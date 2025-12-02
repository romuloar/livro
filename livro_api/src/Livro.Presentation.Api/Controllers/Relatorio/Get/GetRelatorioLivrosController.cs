using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Comum;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Relatorio.Get;

[ApiController]
[Route(ApiRoutes.Relatorio.GetLivro)]
[Tags("Relatórios")]
public class GetRelatorioLivrosController : ControllerBase
{
    private readonly IGetRelatorioLivrosUseCase _getRelatorioLivrosUseCase;

    public GetRelatorioLivrosController(IGetRelatorioLivrosUseCase getRelatorioLivrosUseCase)
    {
        _getRelatorioLivrosUseCase = getRelatorioLivrosUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetRelatorioLivros()
    {
        var result = await _getRelatorioLivrosUseCase.ExecuteAsync();
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
