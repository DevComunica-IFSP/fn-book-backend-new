﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FnBook.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncCurrentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    comentario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    usuario_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    noticia_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    comentario_pai_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    texto = table.Column<string>(type: "TEXT", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, defaultValue: "pendente"),
                    moderado_por = table.Column<Guid>(type: "TEXT", nullable: true),
                    data_moderacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.comentario_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario");
        }
    }
}
