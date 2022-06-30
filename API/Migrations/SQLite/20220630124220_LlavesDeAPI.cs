using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class LlavesDeAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LlavesDeAPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    Llave = table.Column<string>(type: "TEXT", nullable: true),
                    PeticionesEnMes = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RolDeAcceso = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LlavesDeAPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LlavesDeAPI_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LlavesDeAPI_IdUsuario",
                table: "LlavesDeAPI",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LlavesDeAPI");
        }
    }
}
