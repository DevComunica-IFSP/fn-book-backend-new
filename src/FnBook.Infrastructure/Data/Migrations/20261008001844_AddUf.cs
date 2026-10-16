using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FnBook.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uf",
                columns: table => new
                {
                    uf_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    sigla = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uf", x => x.uf_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uf_sigla",
                table: "Uf",
                column: "sigla",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Origem_Uf_uf_id",
                table: "Origem",
                column: "uf_id",
                principalTable: "Uf",
                principalColumn: "uf_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Origem_Uf_uf_id",
                table: "Origem");

            migrationBuilder.DropTable(
                name: "Uf");
        }
    }
}
