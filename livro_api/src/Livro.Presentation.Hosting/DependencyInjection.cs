using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Livro.Infra.EfCore.Contexts;

// Domain Ports
using Livro.Domain.Port.Livro.Write.CreateLivro;
using Livro.Domain.Port.Livro.Read.GetAllLivros;
using Livro.Domain.Port.Livro.Read.GetLivroById;
using Livro.Domain.Port.Livro.Write.UpdateLivro;
using Livro.Domain.Port.Livro.Write.DeleteLivro;
using Livro.Domain.Port.Autor.Write.CreateAutor;
using Livro.Domain.Port.Autor.Read.GetAllAutores;
using Livro.Domain.Port.Autor.Read.GetAutorById;
using Livro.Domain.Port.Autor.Write.UpdateAutor;
using Livro.Domain.Port.Autor.Write.DeleteAutor;
using Livro.Domain.Port.Assunto.Write.CreateAssunto;
using Livro.Domain.Port.Assunto.Read.GetAllAssuntos;
using Livro.Domain.Port.Assunto.Read.GetAssuntoById;
using Livro.Domain.Port.Assunto.Write.UpdateAssunto;
using Livro.Domain.Port.Assunto.Write.DeleteAssunto;
using Livro.Domain.Port.TipoCompra.GetAllTiposCompra;
using Livro.Domain.Port.Relatorio.GetRelatorioLivros;

// Infrastructure Adapters
using Livro.Infra.EfCore.Adapter.Livro.Write.CreateLivro;
using Livro.Infra.EfCore.Adapter.Livro.Read.GetAllLivros;
using Livro.Infra.EfCore.Adapter.Livro.Read.GetLivroById;
using Livro.Infra.EfCore.Adapter.Livro.Write.UpdateLivro;
using Livro.Infra.EfCore.Adapter.Livro.Write.DeleteLivro;
using Livro.Infra.EfCore.Adapter.Autor.Write.CreateAutor;
using Livro.Infra.EfCore.Adapter.Autor.Read.GetAllAutores;
using Livro.Infra.EfCore.Adapter.Autor.Read.GetAutorById;
using Livro.Infra.EfCore.Adapter.Autor.Write.UpdateAutor;
using Livro.Infra.EfCore.Adapter.Autor.Write.DeleteAutor;
using Livro.Infra.EfCore.Adapter.Assunto.Write.CreateAssunto;
using Livro.Infra.EfCore.Adapter.Assunto.Read.GetAllAssuntos;
using Livro.Infra.EfCore.Adapter.Assunto.Read.GetAssuntoById;
using Livro.Infra.EfCore.Adapter.Assunto.Write.UpdateAssunto;
using Livro.Infra.EfCore.Adapter.Assunto.Write.DeleteAssunto;
using Livro.Infra.EfCore.Adapter.TipoCompra.Read.GetAllTiposCompra;
using Livro.Infra.EfCore.Adapter.Relatorio.Read.GetRelatorioLivros;

// Application UseCase
using Livro.Application.UseCase.Livro.Write.CreateLivro;
using Livro.Application.UseCase.Livro.Read.GetAllLivros;
using Livro.Application.UseCase.Livro.Read.GetLivroById;
using Livro.Application.UseCase.Livro.Write.UpdateLivro;
using Livro.Application.UseCase.Livro.Write.DeleteLivro;
using Livro.Application.UseCase.Autor.Write.CreateAutor;
using Livro.Application.UseCase.Autor.Read.GetAllAutores;
using Livro.Application.UseCase.Autor.Read.GetAutorById;
using Livro.Application.UseCase.Autor.Write.UpdateAutor;
using Livro.Application.UseCase.Autor.Write.DeleteAutor;
using Livro.Application.UseCase.Assunto.Write.CreateAssunto;
using Livro.Application.UseCase.Assunto.Read.GetAllAssuntos;
using Livro.Application.UseCase.Assunto.Read.GetAssuntoById;
using Livro.Application.UseCase.Assunto.Write.UpdateAssunto;
using Livro.Application.UseCase.Assunto.Write.DeleteAssunto;
using Livro.Application.UseCase.Comum;

namespace Livro.Presentation.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddLivroServices(this IServiceCollection services, string connectionString)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        // Ports (Infrastructure) - Livro
        services.AddScoped<ICreateLivroPort, CreateLivroPortAdapter>();
        services.AddScoped<IGetAllLivrosPort, GetAllLivrosPortAdapter>();
        services.AddScoped<IGetLivroByIdPort, GetLivroByIdPortAdapter>();
        services.AddScoped<IUpdateLivroPort, UpdateLivroPortAdapter>();
        services.AddScoped<IDeleteLivroPort, DeleteLivroPortAdapter>();

        // Ports (Infrastructure) - Autor
        services.AddScoped<ICreateAutorPort, CreateAutorPortAdapter>();
        services.AddScoped<IGetAllAutoresPort, GetAllAutoresPortAdapter>();
        services.AddScoped<IGetAutorByIdPort, GetAutorByIdPortAdapter>();
        services.AddScoped<IUpdateAutorPort, UpdateAutorPortAdapter>();
        services.AddScoped<IDeleteAutorPort, DeleteAutorPortAdapter>();

        // Ports (Infrastructure) - Assunto
        services.AddScoped<ICreateAssuntoPort, CreateAssuntoPortAdapter>();
        services.AddScoped<IGetAllAssuntosPort, GetAllAssuntosPortAdapter>();
        services.AddScoped<IGetAssuntoByIdPort, GetAssuntoByIdPortAdapter>();
        services.AddScoped<IUpdateAssuntoPort, UpdateAssuntoPortAdapter>();
        services.AddScoped<IDeleteAssuntoPort, DeleteAssuntoPortAdapter>();

        // Ports (Infrastructure) - TipoCompra e Relatorio
        services.AddScoped<IGetAllTiposCompraPort, GetAllTiposCompraPortAdapter>();
        services.AddScoped<IGetRelatorioLivrosPort, GetRelatorioLivrosPortAdapter>();

        // UseCases (Application)
        services.AddScoped<ICreateLivroUseCase, CreateLivroUseCase>();
        services.AddScoped<IGetAllLivrosUseCase, GetAllLivrosUseCase>();
        services.AddScoped<IGetLivroByIdUseCase, GetLivroByIdUseCase>();
        services.AddScoped<IUpdateLivroUseCase, UpdateLivroUseCase>();
        services.AddScoped<IDeleteLivroUseCase, DeleteLivroUseCase>();

        services.AddScoped<ICreateAutorUseCase, CreateAutorUseCase>();
        services.AddScoped<IGetAllAutoresUseCase, GetAllAutoresUseCase>();
        services.AddScoped<IGetAutorByIdUseCase, GetAutorByIdUseCase>();
        services.AddScoped<IUpdateAutorUseCase, UpdateAutorUseCase>();
        services.AddScoped<IDeleteAutorUseCase, DeleteAutorUseCase>();

        services.AddScoped<ICreateAssuntoUseCase, CreateAssuntoUseCase>();
        services.AddScoped<IGetAllAssuntosUseCase, GetAllAssuntosUseCase>();
        services.AddScoped<IGetAssuntoByIdUseCase, GetAssuntoByIdUseCase>();
        services.AddScoped<IUpdateAssuntoUseCase, UpdateAssuntoUseCase>();
        services.AddScoped<IDeleteAssuntoUseCase, DeleteAssuntoUseCase>();

        services.AddScoped<IGetAllTiposCompraUseCase, GetAllTiposCompraUseCase>();
        services.AddScoped<IGetRelatorioLivrosUseCase, GetRelatorioLivrosUseCase>();

        return services;
    }
}
