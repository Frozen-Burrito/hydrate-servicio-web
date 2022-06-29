using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class AgregarDatosDePerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatosDeActividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    METs = table.Column<double>(type: "REAL", nullable: false),
                    VelocidadPromedioKMH = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosDeActividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatosMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Hipervolemia = table.Column<float>(type: "REAL", nullable: false),
                    PesoPostDialisis = table.Column<float>(type: "REAL", nullable: false),
                    AguaExtracelular = table.Column<float>(type: "REAL", nullable: false),
                    Normovolemia = table.Column<float>(type: "REAL", nullable: false),
                    GananciaRegistrada = table.Column<float>(type: "REAL", nullable: false),
                    GananciaReal = table.Column<float>(type: "REAL", nullable: false),
                    FechaProxCita = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosMedicos", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_DatosMedicos_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Valor = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_Etiquetas_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HabitosSemanales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    HorasDeSuenio = table.Column<double>(type: "REAL", nullable: false),
                    HorasDeActividadFisica = table.Column<double>(type: "REAL", nullable: false),
                    HorasDeOcupacion = table.Column<double>(type: "REAL", nullable: false),
                    TemperaturaMaxima = table.Column<double>(type: "REAL", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitosSemanales", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_HabitosSemanales_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    id_perfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Plazo = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    recompensa = table.Column<int>(type: "INTEGER", nullable: false),
                    cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Notas = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => new { x.Id, x.id_perfil });
                    table.ForeignKey(
                        name: "FK_Metas_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RegistrosDeHidratacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    id_perfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    CantidadEnMl = table.Column<int>(type: "INTEGER", nullable: false),
                    PorcentajeCargaBateria = table.Column<int>(type: "INTEGER", nullable: false),
                    TemperaturaAproximada = table.Column<double>(type: "REAL", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosDeHidratacion", x => new { x.Id, x.id_perfil });
                    table.ForeignKey(
                        name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RegistrosDeActFisica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Fecha = table.Column<string>(type: "TEXT", nullable: true),
                    Duracion = table.Column<int>(type: "INTEGER", nullable: false),
                    Distancia = table.Column<double>(type: "REAL", nullable: false),
                    KcalQuemadas = table.Column<int>(type: "INTEGER", nullable: false),
                    FueAlAireLibre = table.Column<bool>(type: "INTEGER", nullable: false),
                    DatosActividadId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosDeActFisica", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_RegistrosDeActFisica_DatosDeActividades_DatosActividadId",
                        column: x => x.DatosActividadId,
                        principalTable: "DatosDeActividades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrosDeActFisica_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas_De_Meta",
                columns: table => new
                {
                    EtiquetasId = table.Column<int>(type: "INTEGER", nullable: false),
                    EtiquetasIdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    MetasId = table.Column<int>(type: "INTEGER", nullable: false),
                    MetasIdPerfil = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas_De_Meta", x => new { x.EtiquetasId, x.EtiquetasIdPerfil, x.MetasId, x.MetasIdPerfil });
                    table.ForeignKey(
                        name: "FK_Etiquetas_De_Meta_Etiquetas_EtiquetasId_EtiquetasIdPerfil",
                        columns: x => new { x.EtiquetasId, x.EtiquetasIdPerfil },
                        principalTable: "Etiquetas",
                        principalColumns: new[] { "Id", "IdPerfil" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Etiquetas_De_Meta_Metas_MetasId_MetasIdPerfil",
                        columns: x => new { x.MetasId, x.MetasIdPerfil },
                        principalTable: "Metas",
                        principalColumns: new[] { "Id", "id_perfil" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RutinasDeActFisica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    id_perfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Dias = table.Column<int>(type: "INTEGER", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    id_actividad = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutinasDeActFisica", x => new { x.Id, x.id_perfil });
                    table.ForeignKey(
                        name: "FK_RutinasDeActFisica_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RutinasDeActFisica_RegistrosDeActFisica_id_actividad_id_perfil",
                        columns: x => new { x.id_actividad, x.id_perfil },
                        principalTable: "RegistrosDeActFisica",
                        principalColumns: new[] { "Id", "IdPerfil" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_IdCuentaUsuario",
                table: "Perfiles",
                column: "IdCuentaUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatosMedicos_PerfilDeUsuarioId",
                table: "DatosMedicos",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_PerfilDeUsuarioId",
                table: "Etiquetas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_De_Meta_MetasId_MetasIdPerfil",
                table: "Etiquetas_De_Meta",
                columns: new[] { "MetasId", "MetasIdPerfil" });

            migrationBuilder.CreateIndex(
                name: "IX_HabitosSemanales_PerfilDeUsuarioId",
                table: "HabitosSemanales",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_PerfilDeUsuarioId",
                table: "Metas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeActFisica_DatosActividadId",
                table: "RegistrosDeActFisica",
                column: "DatosActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeActFisica_PerfilDeUsuarioId",
                table: "RegistrosDeActFisica",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeHidratacion_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RutinasDeActFisica_id_actividad_id_perfil",
                table: "RutinasDeActFisica",
                columns: new[] { "id_actividad", "id_perfil" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RutinasDeActFisica_PerfilDeUsuarioId",
                table: "RutinasDeActFisica",
                column: "PerfilDeUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Usuarios_IdCuentaUsuario",
                table: "Perfiles",
                column: "IdCuentaUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Usuarios_IdCuentaUsuario",
                table: "Perfiles");

            migrationBuilder.DropTable(
                name: "DatosMedicos");

            migrationBuilder.DropTable(
                name: "Etiquetas_De_Meta");

            migrationBuilder.DropTable(
                name: "HabitosSemanales");

            migrationBuilder.DropTable(
                name: "RegistrosDeHidratacion");

            migrationBuilder.DropTable(
                name: "RutinasDeActFisica");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "RegistrosDeActFisica");

            migrationBuilder.DropTable(
                name: "DatosDeActividades");

            migrationBuilder.DropIndex(
                name: "IX_Perfiles_IdCuentaUsuario",
                table: "Perfiles");
        }
    }
}
