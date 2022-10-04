using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class ArreglarPKDeRegistroHidr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeHidratacion_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeHidratacion_id_perfil",
                table: "RegistrosDeHidratacion",
                column: "id_perfil");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_id_perfil",
                table: "RegistrosDeHidratacion",
                column: "id_perfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_id_perfil",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeHidratacion_id_perfil",
                table: "RegistrosDeHidratacion");

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeHidratacion_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                column: "PerfilDeUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");
        }
    }
}
