using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Livro.Read.GetAllLivros;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Livro.Get;

[ApiController]
[Route(ApiRoutes.Livro.GetAll)]
[Tags("Livros")]
public class GetAllLivrosController : ControllerBase
{
    private readonly IGetAllLivrosUseCase _getAllLivrosUseCase;

    public GetAllLivrosController(IGetAllLivrosUseCase getAllLivrosUseCase)
    {
        _getAllLivrosUseCase = getAllLivrosUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllLivrosUseCase.ExecuteAsync();
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
