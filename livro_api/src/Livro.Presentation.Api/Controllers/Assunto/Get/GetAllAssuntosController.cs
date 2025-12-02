using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Assunto.Read.GetAllAssuntos;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Assunto.Get;

[ApiController]
[Route(ApiRoutes.Assunto.GetAll)]
[Tags("Assuntos")]
public class GetAllAssuntosController : ControllerBase
{
    private readonly IGetAllAssuntosUseCase _getAllAssuntosUseCase;

    public GetAllAssuntosController(IGetAllAssuntosUseCase getAllAssuntosUseCase)
    {
        _getAllAssuntosUseCase = getAllAssuntosUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllAssuntosUseCase.ExecuteAsync();
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
