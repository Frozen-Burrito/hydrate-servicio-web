using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class CambiosConfiguracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificacionesPermitidas",
                table: "Configuracion",
                newName: "PreferenciasDeNotificaciones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferenciasDeNotificaciones",
                table: "Configuracion",
                newName: "NotificacionesPermitidas");
        }
    }
}
