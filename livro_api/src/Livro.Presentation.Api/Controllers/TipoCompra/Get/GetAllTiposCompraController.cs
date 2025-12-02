using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Comum;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.TipoCompra.Get;

[ApiController]
[Route(ApiRoutes.TipoCompra.GetAll)]
[Tags("Tipos de Compra")]
public class GetAllTiposCompraController : ControllerBase
{
    private readonly IGetAllTiposCompraUseCase _getAllTiposCompraUseCase;

    public GetAllTiposCompraController(IGetAllTiposCompraUseCase getAllTiposCompraUseCase)
    {
        _getAllTiposCompraUseCase = getAllTiposCompraUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllTiposCompraUseCase.ExecuteAsync();
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}
