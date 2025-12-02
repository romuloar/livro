namespace Livro.Presentation.Api.Constants;

public static class ApiRoutes
{
    private const string ApiBase = "api";

    public static class Assunto
    {
        private const string Base = $"{ApiBase}/assunto";
        
        public const string GetAll = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Autor
    {
        private const string Base = $"{ApiBase}/autor";
        
        public const string GetAll = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Livro
    {
        private const string Base = $"{ApiBase}/livro";
        
        public const string GetAll = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class TipoCompra
    {
        private const string Base = $"{ApiBase}/tipo-compra";
        
        public const string GetAll = Base;
    }

    public static class Relatorio
    {
        private const string Base = $"{ApiBase}/relatorio";
        
        public const string GetLivro = $"{Base}/livro";
    }
}
