using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class ModificarLlavesDeApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErroresEnMes",
                table: "LlavesDeAPI",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NombreDelCliente",
                table: "LlavesDeAPI",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErroresEnMes",
                table: "LlavesDeAPI");

            migrationBuilder.DropColumn(
                name: "NombreDelCliente",
                table: "LlavesDeAPI");
        }
    }
}
