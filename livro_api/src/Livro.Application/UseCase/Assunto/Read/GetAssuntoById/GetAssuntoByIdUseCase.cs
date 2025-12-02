using Livro.Domain.Port.Assunto.Read.GetAssuntoById;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById.In;
using Livro.Domain.Entity.Assunto;
using Rom.Result;
using Rom.Result.Domain;

namespace Livro.Application.UseCase.Assunto.Read.GetAssuntoById;

public class GetAssuntoByIdUseCase : IGetAssuntoByIdUseCase
{
    private readonly IGetAssuntoByIdPort _port;

    public GetAssuntoByIdUseCase(IGetAssuntoByIdPort port)
    {
        _port = port;
    }

    public async Task<ResultDetail<AssuntoDomain>> ExecuteAsync(GetAssuntoByIdIn input) => await _port.ExecuteAsync(input);
}
