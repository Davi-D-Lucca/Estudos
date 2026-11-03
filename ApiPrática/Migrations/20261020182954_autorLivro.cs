using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiPrática.Migrations
{
    /// <inheritdoc />
    public partial class autorLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAutor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Livros",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Copia os nomes distintos de NameArtist para a tabela Autor
            migrationBuilder.Sql(@"INSERT INTO ""Autor"" (""NameAutor"")
                                   SELECT DISTINCT ""NameArtist"" FROM ""Livros"";");

            // Liga cada livro ao autor com o mesmo nome
            migrationBuilder.Sql(@"UPDATE ""Livros"" l SET ""AutorId"" = a.""Id""
                                   FROM ""Autor"" a WHERE a.""NameAutor"" = l.""NameArtist"";");

            migrationBuilder.DropColumn(
    name: "NameArtist",
    table: "Livros");


            migrationBuilder.CreateIndex(
                name: "IX_Livros_AutorId",
                table: "Livros",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Autor_AutorId",
                table: "Livros",
                column: "AutorId",
                principalTable: "Autor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Autor_AutorId",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropIndex(
                name: "IX_Livros_AutorId",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Livros");

            migrationBuilder.AddColumn<string>(
                name: "NameArtist",
                table: "Livros",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
