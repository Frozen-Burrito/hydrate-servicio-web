using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioHydrate.Migrations
{
    public partial class CreacionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FechaPublicacion = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "Usuarios",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
            //         NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Password = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Usuarios", x => x.Id);
            //     });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
