using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FnBook.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_moderacao",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "moderado_por",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Comentario");

            migrationBuilder.CreateTable(
                name: "Denuncia_Comentario",
                columns: table => new
                {
                    denuncia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    comentario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    motivo = table.Column<string>(type: "TEXT", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncia_Comentario", x => x.denuncia_id);
                    table.ForeignKey(
                        name: "FK_Denuncia_Comentario_Comentario_comentario_id",
                        column: x => x.comentario_id,
                        principalTable: "Comentario",
                        principalColumn: "comentario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Denuncia_Comentario_Usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuario",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fonte_Consultada",
                columns: table => new
                {
                    fonte_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    noticia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    url = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    resumo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fonte_Consultada", x => x.fonte_id);
                    table.ForeignKey(
                        name: "FK_Fonte_Consultada_Noticia_noticia_id",
                        column: x => x.noticia_id,
                        principalTable: "Noticia",
                        principalColumn: "noticia_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Noticia_Similar",
                columns: table => new
                {
                    noticia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    noticia_similar_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    similaridade = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticia_Similar", x => new { x.noticia_id, x.noticia_similar_id });
                    table.CheckConstraint("CK_Noticia_Similar_Distinct", "noticia_id <> noticia_similar_id");
                    table.ForeignKey(
                        name: "FK_Noticia_Similar_Noticia_noticia_id",
                        column: x => x.noticia_id,
                        principalTable: "Noticia",
                        principalColumn: "noticia_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Noticia_Similar_Noticia_noticia_similar_id",
                        column: x => x.noticia_similar_id,
                        principalTable: "Noticia",
                        principalColumn: "noticia_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacao",
                columns: table => new
                {
                    notificacao_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    comentario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    lida = table.Column<bool>(type: "INTEGER", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacao", x => x.notificacao_id);
                    table.ForeignKey(
                        name: "FK_Notificacao_Comentario_comentario_id",
                        column: x => x.comentario_id,
                        principalTable: "Comentario",
                        principalColumn: "comentario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificacao_Usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuario",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Denuncia_Comentario_comentario_id",
                table: "Denuncia_Comentario",
                column: "comentario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncia_Comentario_usuario_id",
                table: "Denuncia_Comentario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Fonte_Consultada_noticia_id",
                table: "Fonte_Consultada",
                column: "noticia_id");

            migrationBuilder.CreateIndex(
                name: "IX_Noticia_Similar_noticia_similar_id",
                table: "Noticia_Similar",
                column: "noticia_similar_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_comentario_id",
                table: "Notificacao",
                column: "comentario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_usuario_id",
                table: "Notificacao",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Denuncia_Comentario");

            migrationBuilder.DropTable(
                name: "Fonte_Consultada");

            migrationBuilder.DropTable(
                name: "Noticia_Similar");

            migrationBuilder.DropTable(
                name: "Notificacao");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_moderacao",
                table: "Comentario",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "moderado_por",
                table: "Comentario",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Comentario",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "pendente");
        }
    }
}
