using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Autor.Read.GetAllAutores;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Autor.Get;

[ApiController]
[Route(ApiRoutes.Autor.GetAll)]
[Tags("Autores")]
public class GetAllAutoresController : ControllerBase
{
    private readonly IGetAllAutoresUseCase _getAllAutoresUseCase;

    public GetAllAutoresController(IGetAllAutoresUseCase getAllAutoresUseCase)
    {
        _getAllAutoresUseCase = getAllAutoresUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllAutoresUseCase.ExecuteAsync();
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
