using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class ComentariosPendientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NecesitaModificaciones",
                table: "Comentarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NecesitaModificaciones",
                table: "Comentarios");
        }
    }
}
