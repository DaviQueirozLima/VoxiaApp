using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxia.Infrastructure.Migrations
{
    public partial class CardUsuarioOpcionalFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cria a coluna UsuarioId como nullable
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Cards",
                type: "uuid",
                nullable: true);

            // Adiciona a FK com SetNull
            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Usuarios_UsuarioId",
                table: "Cards",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a FK
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Usuarios_UsuarioId",
                table: "Cards");

            // Remove a coluna
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Cards");
        }
    }
}
