using Livro.Domain.Entity.Livro;
using Livro.Domain.Entity.Autor;
using Livro.Domain.Entity.Assunto;
using Livro.Domain.Entity.TipoCompra;
using Livro.Domain.Entity.LivroAutor;
using Livro.Domain.Entity.LivroAssunto;
using Livro.Domain.Entity.LivroValor;

namespace Livro.Infra.EfCore.Entities;

/// <summary>
/// Métodos de extensão para conversão entre Entities do EF Core e Entities do Domain.
/// Isso permite que os Adapters trabalhem com EF Core Entity mas retornem Domain Entity.
/// </summary>
public static class EntityExtensions
{
    // LivroEntity -> LivroDomain
    public static LivroDomain ToDomain(this LivroEntity entity)
    {
        return new LivroDomain
        {
            Codl = entity.Codl,
            Titulo = entity.Titulo,
            Editora = entity.Editora,
            Edicao = entity.Edicao,
            AnoPublicacao = entity.AnoPublicacao
        };
    }

    // LivroDomain -> LivroEntity
    public static LivroEntity ToEntity(this LivroDomain domain)
    {
        return new LivroEntity
        {
            Codl = domain.Codl,
            Titulo = domain.Titulo,
            Editora = domain.Editora,
            Edicao = domain.Edicao,
            AnoPublicacao = domain.AnoPublicacao
        };
    }

    // AutorEntity -> AutorDomain
    public static AutorDomain ToDomain(this AutorEntity entity)
    {
        return new AutorDomain
        {
            CodAu = entity.CodAu,
            Nome = entity.Nome
        };
    }

    // AutorDomain -> AutorEntity
    public static AutorEntity ToEntity(this AutorDomain domain)
    {
        return new AutorEntity
        {
            CodAu = domain.CodAu,
            Nome = domain.Nome
        };
    }

    // AssuntoEntity -> AssuntoDomain
    public static AssuntoDomain ToDomain(this AssuntoEntity entity)
    {
        return new AssuntoDomain
        {
            CodAs = entity.CodAs,
            Descricao = entity.Descricao
        };
    }

    // AssuntoDomain -> AssuntoEntity
    public static AssuntoEntity ToEntity(this AssuntoDomain domain)
    {
        return new AssuntoEntity
        {
            CodAs = domain.CodAs,
            Descricao = domain.Descricao
        };
    }

    // TipoCompraEntity -> TipoCompraDomain
    public static TipoCompraDomain ToDomain(this TipoCompraEntity entity)
    {
        return new TipoCompraDomain
        {
            CodTc = entity.CodTc,
            Descricao = entity.Descricao
        };
    }

    // LivroAutorEntity -> LivroAutorDomain
    public static LivroAutorDomain ToDomain(this LivroAutorEntity entity)
    {
        return new LivroAutorDomain
        {
            Livro_Codl = entity.Livro_Codl,
            Autor_CodAu = entity.Autor_CodAu
        };
    }

    // LivroAutorDomain -> LivroAutorEntity
    public static LivroAutorEntity ToEntity(this LivroAutorDomain domain)
    {
        return new LivroAutorEntity
        {
            Livro_Codl = domain.Livro_Codl,
            Autor_CodAu = domain.Autor_CodAu
        };
    }

    // LivroAssuntoEntity -> LivroAssuntoDomain
    public static LivroAssuntoDomain ToDomain(this LivroAssuntoEntity entity)
    {
        return new LivroAssuntoDomain
        {
            Livro_Codl = entity.Livro_Codl,
            Assunto_CodAs = entity.Assunto_CodAs
        };
    }

    // LivroAssuntoDomain -> LivroAssuntoEntity
    public static LivroAssuntoEntity ToEntity(this LivroAssuntoDomain domain)
    {
        return new LivroAssuntoEntity
        {
            Livro_Codl = domain.Livro_Codl,
            Assunto_CodAs = domain.Assunto_CodAs
        };
    }

    // LivroValorEntity -> LivroValorDomain
    public static LivroValorDomain ToDomain(this LivroValorEntity entity)
    {
        return new LivroValorDomain
        {
            Codlv = entity.Codlv,
            Livro_Codl = entity.Livro_Codl,
            TipoCompra_CodTc = entity.TipoCompra_CodTc,
            Valor = entity.Valor
        };
    }

    // LivroValorDomain -> LivroValorEntity
    public static LivroValorEntity ToEntity(this LivroValorDomain domain)
    {
        return new LivroValorEntity
        {
            Codlv = domain.Codlv,
            Livro_Codl = domain.Livro_Codl,
            TipoCompra_CodTc = domain.TipoCompra_CodTc,
            Valor = domain.Valor
        };
    }
}
