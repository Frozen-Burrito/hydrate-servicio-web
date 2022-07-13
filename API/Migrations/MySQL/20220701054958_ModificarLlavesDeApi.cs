using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.MySQL
{
    public partial class ModificarLlavesDeApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErroresEnMes",
                table: "LlavesDeAPI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NombreDelCliente",
                table: "LlavesDeAPI",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
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
