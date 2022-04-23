using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class TablasComentarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Usuarios_AutorId",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentariosReportados_Comentario_ComentariosReportadosId",
                table: "ComentariosReportados");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentariosUtiles_Comentario_ComentariosUtilesId",
                table: "ComentariosUtiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuesta_Comentario_ComentarioId",
                table: "Respuesta");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuesta_Usuarios_AutorId",
                table: "Respuesta");

            migrationBuilder.DropForeignKey(
                name: "FK_RespuestasReportadas_Respuesta_RespuestasReportadasId",
                table: "RespuestasReportadas");

            migrationBuilder.DropForeignKey(
                name: "FK_RespuestasUtiles_Respuesta_RespuestasUtilesId",
                table: "RespuestasUtiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Respuesta",
                table: "Respuesta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario");

            migrationBuilder.RenameTable(
                name: "Respuesta",
                newName: "Respuestas");

            migrationBuilder.RenameTable(
                name: "Comentario",
                newName: "Comentarios");

            migrationBuilder.RenameIndex(
                name: "IX_Respuesta_ComentarioId",
                table: "Respuestas",
                newName: "IX_Respuestas_ComentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Respuesta_AutorId",
                table: "Respuestas",
                newName: "IX_Respuestas_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comentario_AutorId",
                table: "Comentarios",
                newName: "IX_Comentarios_AutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Respuestas",
                table: "Respuestas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Usuarios_AutorId",
                table: "Comentarios",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentariosReportados_Comentarios_ComentariosReportadosId",
                table: "ComentariosReportados",
                column: "ComentariosReportadosId",
                principalTable: "Comentarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentariosUtiles_Comentarios_ComentariosUtilesId",
                table: "ComentariosUtiles",
                column: "ComentariosUtilesId",
                principalTable: "Comentarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Comentarios_ComentarioId",
                table: "Respuestas",
                column: "ComentarioId",
                principalTable: "Comentarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Usuarios_AutorId",
                table: "Respuestas",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RespuestasReportadas_Respuestas_RespuestasReportadasId",
                table: "RespuestasReportadas",
                column: "RespuestasReportadasId",
                principalTable: "Respuestas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RespuestasUtiles_Respuestas_RespuestasUtilesId",
                table: "RespuestasUtiles",
                column: "RespuestasUtilesId",
                principalTable: "Respuestas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Usuarios_AutorId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentariosReportados_Comentarios_ComentariosReportadosId",
                table: "ComentariosReportados");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentariosUtiles_Comentarios_ComentariosUtilesId",
                table: "ComentariosUtiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Comentarios_ComentarioId",
                table: "Respuestas");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Usuarios_AutorId",
                table: "Respuestas");

            migrationBuilder.DropForeignKey(
                name: "FK_RespuestasReportadas_Respuestas_RespuestasReportadasId",
                table: "RespuestasReportadas");

            migrationBuilder.DropForeignKey(
                name: "FK_RespuestasUtiles_Respuestas_RespuestasUtilesId",
                table: "RespuestasUtiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Respuestas",
                table: "Respuestas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios");

            migrationBuilder.RenameTable(
                name: "Respuestas",
                newName: "Respuesta");

            migrationBuilder.RenameTable(
                name: "Comentarios",
                newName: "Comentario");

            migrationBuilder.RenameIndex(
                name: "IX_Respuestas_ComentarioId",
                table: "Respuesta",
                newName: "IX_Respuesta_ComentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Respuestas_AutorId",
                table: "Respuesta",
                newName: "IX_Respuesta_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_AutorId",
                table: "Comentario",
                newName: "IX_Comentario_AutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Respuesta",
                table: "Respuesta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Usuarios_AutorId",
                table: "Comentario",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentariosReportados_Comentario_ComentariosReportadosId",
                table: "ComentariosReportados",
                column: "ComentariosReportadosId",
                principalTable: "Comentario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentariosUtiles_Comentario_ComentariosUtilesId",
                table: "ComentariosUtiles",
                column: "ComentariosUtilesId",
                principalTable: "Comentario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuesta_Comentario_ComentarioId",
                table: "Respuesta",
                column: "ComentarioId",
                principalTable: "Comentario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuesta_Usuarios_AutorId",
                table: "Respuesta",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RespuestasReportadas_Respuesta_RespuestasReportadasId",
                table: "RespuestasReportadas",
                column: "RespuestasReportadasId",
                principalTable: "Respuesta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RespuestasUtiles_Respuesta_RespuestasUtilesId",
                table: "RespuestasUtiles",
                column: "RespuestasUtilesId",
                principalTable: "Respuesta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
