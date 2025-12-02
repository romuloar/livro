using Microsoft.AspNetCore.Mvc;
using Livro.Application.UseCase.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Write.CreateAssunto.In;
using Livro.Presentation.Api.Constants;
using Rom.Result;

namespace Livro.Presentation.Api.Controllers.Assunto.Post;

[ApiController]
[Route(ApiRoutes.Assunto.Create)]
[Tags("Assuntos")]
public class CreateAssuntoController : ControllerBase
{
    private readonly ICreateAssuntoUseCase _createAssuntoUseCase;

    public CreateAssuntoController(ICreateAssuntoUseCase createAssuntoUseCase)
    {
        _createAssuntoUseCase = createAssuntoUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssuntoIn input)
    {
        var result = await _createAssuntoUseCase.ExecuteAsync(input);
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Created($"{ApiRoutes.Assunto.GetAll}/{result.ResultData?.CodAs}", result);
    }
}
