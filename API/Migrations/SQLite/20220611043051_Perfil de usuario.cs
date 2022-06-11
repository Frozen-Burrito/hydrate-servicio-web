using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class Perfildeusuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCuentaUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    Apellido = table.Column<string>(type: "TEXT", nullable: true),
                    FechaNacimiento = table.Column<string>(type: "TEXT", nullable: true),
                    SexoUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Estatura = table.Column<double>(type: "REAL", nullable: false),
                    Peso = table.Column<double>(type: "REAL", nullable: false),
                    Ocupacion = table.Column<int>(type: "INTEGER", nullable: false),
                    CondicionMedica = table.Column<int>(type: "INTEGER", nullable: false),
                    PaisDeResidenciaId = table.Column<int>(type: "INTEGER", nullable: true),
                    CantidadMonedas = table.Column<int>(type: "INTEGER", nullable: false),
                    NumModificaciones = table.Column<int>(type: "INTEGER", nullable: false),
                    IdEntornoSeleccionado = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perfiles_Paises_PaisDeResidenciaId",
                        column: x => x.PaisDeResidenciaId,
                        principalTable: "Paises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entornos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UrlImagen = table.Column<string>(type: "TEXT", nullable: true),
                    PrecioEnMonedas = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entornos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entornos_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entornos_PerfilId",
                table: "Entornos",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_PaisDeResidenciaId",
                table: "Perfiles",
                column: "PaisDeResidenciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entornos");

            migrationBuilder.DropTable(
                name: "Perfiles");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
