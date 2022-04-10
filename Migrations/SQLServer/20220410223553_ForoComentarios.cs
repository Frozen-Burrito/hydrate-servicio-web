using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioHydrate.Migrations.SQLServer
{
    public partial class ForoComentarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolDeUsuario",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FechaPublicacion",
                table: "Recursos",
                type: "nvarchar(33)",
                maxLength: 33,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Asunto = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Fecha = table.Column<string>(type: "nvarchar(33)", maxLength: 33, nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComentariosReportados",
                columns: table => new
                {
                    ComentariosReportadosId = table.Column<int>(type: "int", nullable: false),
                    ReportesDeUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariosReportados", x => new { x.ComentariosReportadosId, x.ReportesDeUsuariosId });
                    table.ForeignKey(
                        name: "FK_ComentariosReportados_Comentarios_ComentariosReportadosId",
                        column: x => x.ComentariosReportadosId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentariosReportados_Usuarios_ReportesDeUsuariosId",
                        column: x => x.ReportesDeUsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComentariosUtiles",
                columns: table => new
                {
                    ComentariosUtilesId = table.Column<int>(type: "int", nullable: false),
                    UtilParaUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariosUtiles", x => new { x.ComentariosUtilesId, x.UtilParaUsuariosId });
                    table.ForeignKey(
                        name: "FK_ComentariosUtiles_Comentarios_ComentariosUtilesId",
                        column: x => x.ComentariosUtilesId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentariosUtiles_Usuarios_UtilParaUsuariosId",
                        column: x => x.UtilParaUsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contenido = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Fecha = table.Column<string>(type: "nvarchar(33)", maxLength: 33, nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdComentario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respuestas_Comentarios_IdComentario",
                        column: x => x.IdComentario,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Respuestas_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasReportadas",
                columns: table => new
                {
                    ReportesDeUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RespuestasReportadasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasReportadas", x => new { x.ReportesDeUsuariosId, x.RespuestasReportadasId });
                    table.ForeignKey(
                        name: "FK_RespuestasReportadas_Respuestas_RespuestasReportadasId",
                        column: x => x.RespuestasReportadasId,
                        principalTable: "Respuestas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespuestasReportadas_Usuarios_ReportesDeUsuariosId",
                        column: x => x.ReportesDeUsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasUtiles",
                columns: table => new
                {
                    RespuestasUtilesId = table.Column<int>(type: "int", nullable: false),
                    UtilParaUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasUtiles", x => new { x.RespuestasUtilesId, x.UtilParaUsuariosId });
                    table.ForeignKey(
                        name: "FK_RespuestasUtiles_Respuestas_RespuestasUtilesId",
                        column: x => x.RespuestasUtilesId,
                        principalTable: "Respuestas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespuestasUtiles_Usuarios_UtilParaUsuariosId",
                        column: x => x.UtilParaUsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_AutorId",
                table: "Comentarios",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentariosReportados_ReportesDeUsuariosId",
                table: "ComentariosReportados",
                column: "ReportesDeUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentariosUtiles_UtilParaUsuariosId",
                table: "ComentariosUtiles",
                column: "UtilParaUsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_AutorId",
                table: "Respuestas",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_IdComentario",
                table: "Respuestas",
                column: "IdComentario");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasReportadas_RespuestasReportadasId",
                table: "RespuestasReportadas",
                column: "RespuestasReportadasId");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasUtiles_UtilParaUsuariosId",
                table: "RespuestasUtiles",
                column: "UtilParaUsuariosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComentariosReportados");

            migrationBuilder.DropTable(
                name: "ComentariosUtiles");

            migrationBuilder.DropTable(
                name: "RespuestasReportadas");

            migrationBuilder.DropTable(
                name: "RespuestasUtiles");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropColumn(
                name: "RolDeUsuario",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "FechaPublicacion",
                table: "Recursos",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(33)",
                oldMaxLength: 33,
                oldNullable: true);
        }
    }
}
