using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FnBook.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Noticia",
                columns: table => new
                {
                    noticia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    titulo = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    texto = table.Column<string>(type: "TEXT", nullable: false),
                    imagem = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    categoria_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    hash_conteudo = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    cadastrado_por = table.Column<Guid>(type: "TEXT", nullable: true),
                    visualizacoes = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    verdadeiro_ia = table.Column<bool>(type: "INTEGER", nullable: true),
                    justificativa = table.Column<string>(type: "TEXT", nullable: true),
                    modelo_ia = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    confianca_ia = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    data_classificacao_ia = table.Column<DateTime>(type: "TEXT", nullable: true),
                    verdadeiro_final = table.Column<bool>(type: "INTEGER", nullable: true),
                    verificado = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    verificado_por = table.Column<Guid>(type: "TEXT", nullable: true),
                    data_verificacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    embedding = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticia", x => x.noticia_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Origem",
                columns: table => new
                {
                    origem_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    noticia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    uf_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origem", x => x.origem_id);
                    table.ForeignKey(
                        name: "FK_Origem_Noticia_noticia_id",
                        column: x => x.noticia_id,
                        principalTable: "Noticia",
                        principalColumn: "noticia_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Origem_Users_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Origem_noticia_id",
                table: "Origem",
                column: "noticia_id");

            migrationBuilder.CreateIndex(
                name: "IX_Origem_uf_id",
                table: "Origem",
                column: "uf_id");

            migrationBuilder.CreateIndex(
                name: "IX_Origem_usuario_id",
                table: "Origem",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Origem");

            migrationBuilder.DropTable(
                name: "Noticia");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
