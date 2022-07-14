using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.MySQL
{
    public partial class AgregarPerfilesYDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatosDeActividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    METs = table.Column<double>(type: "double", nullable: false),
                    VelocidadPromedioKMH = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosDeActividades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCuentaUsuario = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nombre = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Apellido = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SexoUsuario = table.Column<int>(type: "int", nullable: false),
                    Estatura = table.Column<double>(type: "double", nullable: false),
                    Peso = table.Column<double>(type: "double", nullable: false),
                    Ocupacion = table.Column<int>(type: "int", nullable: false),
                    CondicionMedica = table.Column<int>(type: "int", nullable: false),
                    PaisDeResidenciaId = table.Column<int>(type: "int", nullable: true),
                    CantidadMonedas = table.Column<int>(type: "int", nullable: false),
                    NumModificaciones = table.Column<int>(type: "int", nullable: false),
                    IdEntornoSeleccionado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perfiles_Paises_PaisDeResidenciaId",
                        column: x => x.PaisDeResidenciaId,
                        principalTable: "Paises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Perfiles_Usuarios_IdCuentaUsuario",
                        column: x => x.IdCuentaUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DatosMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    Hipervolemia = table.Column<float>(type: "float", nullable: false),
                    PesoPostDialisis = table.Column<float>(type: "float", nullable: false),
                    AguaExtracelular = table.Column<float>(type: "float", nullable: false),
                    Normovolemia = table.Column<float>(type: "float", nullable: false),
                    GananciaRegistrada = table.Column<float>(type: "float", nullable: false),
                    GananciaReal = table.Column<float>(type: "float", nullable: false),
                    FechaProxCita = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosMedicos", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_DatosMedicos_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entornos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrlImagen = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrecioEnMonedas = table.Column<int>(type: "int", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entornos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entornos_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    Valor = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_Etiquetas_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    id_perfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    Plazo = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    recompensa = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    Notas = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => new { x.Id, x.id_perfil });
                    table.ForeignKey(
                        name: "FK_Metas_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrosDeActFisica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Distancia = table.Column<double>(type: "double", nullable: false),
                    KcalQuemadas = table.Column<int>(type: "int", nullable: false),
                    FueAlAireLibre = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DatosActividadId = table.Column<int>(type: "int", nullable: true),
                    EsInformacionAbierta = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrosDeHidratacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    id_perfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    CantidadEnMl = table.Column<int>(type: "int", nullable: false),
                    PorcentajeCargaBateria = table.Column<int>(type: "int", nullable: false),
                    TemperaturaAproximada = table.Column<double>(type: "double", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EsInformacionAbierta = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosDeHidratacion", x => new { x.Id, x.id_perfil });
                    table.ForeignKey(
                        name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReporteSemanal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    HorasDeSuenio = table.Column<double>(type: "double", nullable: false),
                    HorasDeActividadFisica = table.Column<double>(type: "double", nullable: false),
                    HorasDeOcupacion = table.Column<double>(type: "double", nullable: false),
                    TemperaturaMaxima = table.Column<double>(type: "double", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteSemanal", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_ReporteSemanal_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Etiquetas_De_Meta",
                columns: table => new
                {
                    EtiquetasId = table.Column<int>(type: "int", nullable: false),
                    EtiquetasIdPerfil = table.Column<int>(type: "int", nullable: false),
                    MetasId = table.Column<int>(type: "int", nullable: false),
                    MetasIdPerfil = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RutinasDeActFisica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    id_perfil = table.Column<int>(type: "int", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "int", nullable: true),
                    Dias = table.Column<int>(type: "int", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    id_actividad = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_RutinasDeActFisica_RegistrosDeActFisica_id_actividad_id_perf~",
                        columns: x => new { x.id_actividad, x.id_perfil },
                        principalTable: "RegistrosDeActFisica",
                        principalColumns: new[] { "Id", "IdPerfil" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DatosMedicos_PerfilDeUsuarioId",
                table: "DatosMedicos",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Entornos_PerfilId",
                table: "Entornos",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_PerfilDeUsuarioId",
                table: "Etiquetas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_De_Meta_MetasId_MetasIdPerfil",
                table: "Etiquetas_De_Meta",
                columns: new[] { "MetasId", "MetasIdPerfil" });

            migrationBuilder.CreateIndex(
                name: "IX_Metas_PerfilDeUsuarioId",
                table: "Metas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_IdCuentaUsuario",
                table: "Perfiles",
                column: "IdCuentaUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_PaisDeResidenciaId",
                table: "Perfiles",
                column: "PaisDeResidenciaId");

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
                name: "IX_ReporteSemanal_PerfilDeUsuarioId",
                table: "ReporteSemanal",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatosMedicos");

            migrationBuilder.DropTable(
                name: "Entornos");

            migrationBuilder.DropTable(
                name: "Etiquetas_De_Meta");

            migrationBuilder.DropTable(
                name: "RegistrosDeHidratacion");

            migrationBuilder.DropTable(
                name: "ReporteSemanal");

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

            migrationBuilder.DropTable(
                name: "Perfiles");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
