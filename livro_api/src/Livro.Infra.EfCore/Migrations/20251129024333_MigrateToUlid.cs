using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Livro.Infra.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class MigrateToUlid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assunto",
                columns: table => new
                {
                    CodAs = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assunto", x => x.CodAs);
                });

            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    CodAu = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.CodAu);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    Codl = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Editora = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Edicao = table.Column<int>(type: "INTEGER", nullable: false),
                    AnoPublicacao = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.Codl);
                });

            migrationBuilder.CreateTable(
                name: "TipoCompra",
                columns: table => new
                {
                    CodTc = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCompra", x => x.CodTc);
                });

            migrationBuilder.CreateTable(
                name: "Livro_Assunto",
                columns: table => new
                {
                    Livro_Codl = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Assunto_CodAs = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro_Assunto", x => new { x.Livro_Codl, x.Assunto_CodAs });
                    table.ForeignKey(
                        name: "FK_Livro_Assunto_Assunto_Assunto_CodAs",
                        column: x => x.Assunto_CodAs,
                        principalTable: "Assunto",
                        principalColumn: "CodAs",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livro_Assunto_Livro_Livro_Codl",
                        column: x => x.Livro_Codl,
                        principalTable: "Livro",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livro_Autor",
                columns: table => new
                {
                    Livro_Codl = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Autor_CodAu = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro_Autor", x => new { x.Livro_Codl, x.Autor_CodAu });
                    table.ForeignKey(
                        name: "FK_Livro_Autor_Autor_Autor_CodAu",
                        column: x => x.Autor_CodAu,
                        principalTable: "Autor",
                        principalColumn: "CodAu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livro_Autor_Livro_Livro_Codl",
                        column: x => x.Livro_Codl,
                        principalTable: "Livro",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livro_Valor",
                columns: table => new
                {
                    Codlv = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Livro_Codl = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    TipoCompra_CodTc = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro_Valor", x => x.Codlv);
                    table.ForeignKey(
                        name: "FK_Livro_Valor_Livro_Livro_Codl",
                        column: x => x.Livro_Codl,
                        principalTable: "Livro",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livro_Valor_TipoCompra_TipoCompra_CodTc",
                        column: x => x.TipoCompra_CodTc,
                        principalTable: "TipoCompra",
                        principalColumn: "CodTc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Assunto_Assunto_CodAs",
                table: "Livro_Assunto",
                column: "Assunto_CodAs");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Autor_Autor_CodAu",
                table: "Livro_Autor",
                column: "Autor_CodAu");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Valor_Livro_Codl",
                table: "Livro_Valor",
                column: "Livro_Codl");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Valor_TipoCompra_CodTc",
                table: "Livro_Valor",
                column: "TipoCompra_CodTc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livro_Assunto");

            migrationBuilder.DropTable(
                name: "Livro_Autor");

            migrationBuilder.DropTable(
                name: "Livro_Valor");

            migrationBuilder.DropTable(
                name: "Assunto");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Livro");

            migrationBuilder.DropTable(
                name: "TipoCompra");
        }
    }
}
