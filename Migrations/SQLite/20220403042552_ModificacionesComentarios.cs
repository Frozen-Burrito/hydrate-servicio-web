using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class ModificacionesComentarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Comentarios_ComentarioId",
                table: "Respuestas");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_ComentarioId",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "ComentarioId",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "NumeroDeReportes",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "NumeroDeReportes",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "NumeroDeUtil",
                table: "Comentarios");

            migrationBuilder.RenameColumn(
                name: "NumeroDeUtil",
                table: "Respuestas",
                newName: "IdComentario");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_IdComentario",
                table: "Respuestas",
                column: "IdComentario");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Comentarios_IdComentario",
                table: "Respuestas",
                column: "IdComentario",
                principalTable: "Comentarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Comentarios_IdComentario",
                table: "Respuestas");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_IdComentario",
                table: "Respuestas");

            migrationBuilder.RenameColumn(
                name: "IdComentario",
                table: "Respuestas",
                newName: "NumeroDeUtil");

            migrationBuilder.AddColumn<int>(
                name: "ComentarioId",
                table: "Respuestas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroDeReportes",
                table: "Respuestas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroDeReportes",
                table: "Comentarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroDeUtil",
                table: "Comentarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_ComentarioId",
                table: "Respuestas",
                column: "ComentarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Comentarios_ComentarioId",
                table: "Respuestas",
                column: "ComentarioId",
                principalTable: "Comentarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
