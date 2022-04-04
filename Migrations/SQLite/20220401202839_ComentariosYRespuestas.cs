using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class ComentariosYRespuestas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    FechaPublicacion = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    NombreUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Asunto = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Fecha = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Publicado = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroDeReportes = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroDeUtil = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentario_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComentariosReportados",
                columns: table => new
                {
                    ComentariosReportadosId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportesDeUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariosReportados", x => new { x.ComentariosReportadosId, x.ReportesDeUsuariosId });
                    table.ForeignKey(
                        name: "FK_ComentariosReportados_Comentario_ComentariosReportadosId",
                        column: x => x.ComentariosReportadosId,
                        principalTable: "Comentario",
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
                    ComentariosUtilesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UtilParaUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariosUtiles", x => new { x.ComentariosUtilesId, x.UtilParaUsuariosId });
                    table.ForeignKey(
                        name: "FK_ComentariosUtiles_Comentario_ComentariosUtilesId",
                        column: x => x.ComentariosUtilesId,
                        principalTable: "Comentario",
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Contenido = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Fecha = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Publicado = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroDeReportes = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroDeUtil = table.Column<int>(type: "INTEGER", nullable: false),
                    ComentarioId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuesta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respuesta_Comentario_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuesta_Usuarios_AutorId",
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
                    RespuestasReportadasId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasReportadas", x => new { x.ReportesDeUsuariosId, x.RespuestasReportadasId });
                    table.ForeignKey(
                        name: "FK_RespuestasReportadas_Respuesta_RespuestasReportadasId",
                        column: x => x.RespuestasReportadasId,
                        principalTable: "Respuesta",
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
                    RespuestasUtilesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UtilParaUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasUtiles", x => new { x.RespuestasUtilesId, x.UtilParaUsuariosId });
                    table.ForeignKey(
                        name: "FK_RespuestasUtiles_Respuesta_RespuestasUtilesId",
                        column: x => x.RespuestasUtilesId,
                        principalTable: "Respuesta",
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
                name: "IX_Comentario_AutorId",
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
                name: "IX_Respuesta_AutorId",
                table: "Respuestas",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuesta_ComentarioId",
                table: "Respuesta",
                column: "ComentarioId");

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
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "RespuestasReportadas");

            migrationBuilder.DropTable(
                name: "RespuestasUtiles");

            migrationBuilder.DropTable(
                name: "Respuesta");

            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
