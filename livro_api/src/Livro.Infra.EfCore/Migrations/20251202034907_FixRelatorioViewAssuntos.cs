using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livro.Infra.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixRelatorioViewAssuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Recriar a VIEW com subconsulta para evitar duplicação de assuntos
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RelatorioLivros");
            
            migrationBuilder.Sql(@"
                CREATE VIEW vw_RelatorioLivros AS
                SELECT 
                    a.CodAu,
                    a.Nome AS NomeAutor,
                    l.Codl,
                    l.Titulo,
                    l.Editora,
                    l.Edicao,
                    l.AnoPublicacao,
                    (
                        SELECT GROUP_CONCAT(ass2.Descricao, ', ')
                        FROM Livro_Assunto las2
                        INNER JOIN Assunto ass2 ON las2.Assunto_CodAs = ass2.CodAs
                        WHERE las2.Livro_Codl = l.Codl
                    ) AS Assuntos,
                    COUNT(lv.CodLv) AS QuantidadeTiposCompra,
                    IFNULL(SUM(lv.Valor), 0) AS ValorTotal
                FROM Autor a
                INNER JOIN Livro_Autor la ON a.CodAu = la.Autor_CodAu
                INNER JOIN Livro l ON la.Livro_Codl = l.Codl
                LEFT JOIN Livro_Valor lv ON l.Codl = lv.Livro_Codl
                GROUP BY 
                    a.CodAu,
                    a.Nome,
                    l.Codl,
                    l.Titulo,
                    l.Editora,
                    l.Edicao,
                    l.AnoPublicacao
                ORDER BY a.Nome, l.Titulo
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Voltar para a VIEW original
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RelatorioLivros");
            
            migrationBuilder.Sql(@"
                CREATE VIEW vw_RelatorioLivros AS
                SELECT 
                    a.CodAu,
                    a.Nome AS NomeAutor,
                    l.Codl,
                    l.Titulo,
                    l.Editora,
                    l.Edicao,
                    l.AnoPublicacao,
                    GROUP_CONCAT(ass.Descricao, ', ') AS Assuntos,
                    COUNT(lv.CodLv) AS QuantidadeTiposCompra,
                    IFNULL(SUM(lv.Valor), 0) AS ValorTotal
                FROM Autor a
                LEFT JOIN Livro_Autor la ON a.CodAu = la.Autor_CodAu
                LEFT JOIN Livro l ON la.Livro_Codl = l.Codl
                LEFT JOIN Livro_Assunto las ON l.Codl = las.Livro_Codl
                LEFT JOIN Assunto ass ON las.Assunto_CodAs = ass.CodAs
                LEFT JOIN Livro_Valor lv ON l.Codl = lv.Livro_Codl
                GROUP BY 
                    a.CodAu,
                    a.Nome,
                    l.Codl,
                    l.Titulo,
                    l.Editora,
                    l.Edicao,
                    l.AnoPublicacao
                ORDER BY a.Nome, l.Titulo
            ");
        }
    }
}
