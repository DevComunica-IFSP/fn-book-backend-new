using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FnBook.Infrastructure.Data.Migrations;

// Earlier snapshots describe Usuario, Categoria and the like tables, but the
// corresponding migrations still create Users and omit Categoria and the likes.
// Reconcile that physical schema without changing any already applied migration.
public partial class RepairLegacySchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Usuario",
            columns: table => new
            {
                usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                nome_completo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                senha = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                foto_perfil = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                uf_id = table.Column<Guid>(type: "TEXT", nullable: true),
                tipo_usuario = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                score_contribuidor = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                data_cadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                data_atualizacao = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Usuario", x => x.usuario_id));

        migrationBuilder.Sql("""
            INSERT INTO "Usuario" ("usuario_id", "nome_completo", "email", "senha", "ativo", "data_cadastro", "data_atualizacao", "tipo_usuario")
            SELECT "Id", "Name", "Email", "PasswordHash", "IsActive", "CreatedAt", "UpdatedAt", '' FROM "Users";
            """);
        migrationBuilder.CreateIndex(name: "IX_Usuario_email", table: "Usuario", column: "email", unique: true);
        RebuildOrigins(migrationBuilder, "Usuario", "usuario_id");
        migrationBuilder.DropTable(name: "Users");

        migrationBuilder.CreateTable(
            name: "Categoria",
            columns: table => new
            {
                categoria_id = table.Column<Guid>(type: "TEXT", nullable: false),
                nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Categoria", x => x.categoria_id));

        migrationBuilder.CreateTable(
            name: "Curtida_Comentario",
            columns: table => new
            {
                usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                comentario_id = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Curtida_Comentario", x => new { x.usuario_id, x.comentario_id }));

        migrationBuilder.CreateTable(
            name: "Curtida_Noticia",
            columns: table => new
            {
                usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                noticia_id = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Curtida_Noticia", x => new { x.usuario_id, x.noticia_id }));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Curtida_Comentario");
        migrationBuilder.DropTable(name: "Curtida_Noticia");
        migrationBuilder.DropTable(name: "Categoria");

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
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.Sql("""
            INSERT INTO "Users" ("Id", "Name", "Email", "PasswordHash", "IsActive", "CreatedAt", "UpdatedAt")
            SELECT "usuario_id", "nome_completo", "email", "senha", "ativo", "data_cadastro", "data_atualizacao" FROM "Usuario";
            """);
        migrationBuilder.CreateIndex(name: "IX_Users_Email", table: "Users", column: "Email", unique: true);
        RebuildOrigins(migrationBuilder, "Users", "Id");
        migrationBuilder.DropTable(name: "Usuario");
    }

    private static void RebuildOrigins(MigrationBuilder migrationBuilder, string userTable, string userColumn)
    {
        // Both user tables exist while copying; keep foreign keys enabled and
        // preserve all origins while changing only the user foreign key target.
        migrationBuilder.CreateTable(
            name: "__repair_Origem",
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
                table.ForeignKey(name: $"FK_Origem_{userTable}_usuario_id",
                    column: x => x.usuario_id, principalTable: userTable, principalColumn: userColumn,
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(name: "FK_Origem_Noticia_noticia_id", column: x => x.noticia_id,
                    principalTable: "Noticia", principalColumn: "noticia_id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey(name: "FK_Origem_Uf_uf_id", column: x => x.uf_id,
                    principalTable: "Uf", principalColumn: "uf_id", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.Sql("""
            INSERT INTO "__repair_Origem" ("origem_id", "usuario_id", "noticia_id", "uf_id", "data")
            SELECT "origem_id", "usuario_id", "noticia_id", "uf_id", "data" FROM "Origem";
            """);
        migrationBuilder.DropTable(name: "Origem");
        migrationBuilder.RenameTable(name: "__repair_Origem", newName: "Origem");
        migrationBuilder.CreateIndex(name: "IX_Origem_noticia_id", table: "Origem", column: "noticia_id");
        migrationBuilder.CreateIndex(name: "IX_Origem_uf_id", table: "Origem", column: "uf_id");
        migrationBuilder.CreateIndex(name: "IX_Origem_usuario_id", table: "Origem", column: "usuario_id");
    }
}
