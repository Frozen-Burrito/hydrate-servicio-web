using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.MySQL
{
    public partial class CambiosPerfilesConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatosMedicos_Perfiles_PerfilDeUsuarioId",
                table: "DatosMedicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Entornos_Perfiles_PerfilId",
                table: "Entornos");

            migrationBuilder.Sql("DELETE FROM Entornos", true);

            migrationBuilder.DropForeignKey(
                name: "FK_Etiquetas_Perfiles_PerfilDeUsuarioId",
                table: "Etiquetas");

            migrationBuilder.DropForeignKey(
                name: "FK_Metas_Perfiles_PerfilDeUsuarioId",
                table: "Metas");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Paises_PaisDeResidenciaId",
                table: "Perfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeActFisica_DatosDeActividades_DatosActividadId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeActFisica_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSemanal_Perfiles_PerfilDeUsuarioId",
                table: "ReporteSemanal");

            migrationBuilder.DropForeignKey(
                name: "FK_RutinasDeActFisica_Perfiles_PerfilDeUsuarioId",
                table: "RutinasDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RutinasDeActFisica_RegistrosDeActFisica_id_actividad_id_perf~",
                table: "RutinasDeActFisica");

            migrationBuilder.DropTable(
                name: "DatosDeActividades");

            migrationBuilder.DropTable(
                name: "Etiquetas_De_Meta");

            migrationBuilder.DropIndex(
                name: "IX_RutinasDeActFisica_PerfilDeUsuarioId",
                table: "RutinasDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSemanal_PerfilDeUsuarioId",
                table: "ReporteSemanal");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeHidratacion_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeActFisica_DatosActividadId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeActFisica_PerfilDeUsuarioId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_Perfiles_PaisDeResidenciaId",
                table: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Metas_PerfilDeUsuarioId",
                table: "Metas");

            migrationBuilder.DropIndex(
                name: "IX_Etiquetas_PerfilDeUsuarioId",
                table: "Etiquetas");

            migrationBuilder.DropIndex(
                name: "IX_Entornos_PerfilId",
                table: "Entornos");

            migrationBuilder.DropIndex(
                name: "IX_DatosMedicos_PerfilDeUsuarioId",
                table: "DatosMedicos");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "RutinasDeActFisica");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "ReporteSemanal");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropColumn(
                name: "DatosActividadId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropColumn(
                name: "PaisDeResidenciaId",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "Etiquetas");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Entornos");

            migrationBuilder.DropColumn(
                name: "PerfilDeUsuarioId",
                table: "DatosMedicos");

            migrationBuilder.RenameColumn(
                name: "id_actividad",
                table: "RutinasDeActFisica",
                newName: "IdRegistroActividad");

            migrationBuilder.RenameColumn(
                name: "id_perfil",
                table: "RutinasDeActFisica",
                newName: "IdPerfil");

            migrationBuilder.RenameColumn(
                name: "Dias",
                table: "RutinasDeActFisica",
                newName: "DiasDeOcurrencia");

            migrationBuilder.RenameIndex(
                name: "IX_RutinasDeActFisica_id_actividad_id_perfil",
                table: "RutinasDeActFisica",
                newName: "IX_RutinasDeActFisica_IdRegistroActividad_IdPerfil");

            migrationBuilder.RenameColumn(
                name: "id_perfil",
                table: "RegistrosDeHidratacion",
                newName: "IdPerfil");

            migrationBuilder.RenameColumn(
                name: "recompensa",
                table: "Metas",
                newName: "RecomensaMonedas");

            migrationBuilder.RenameColumn(
                name: "fecha_fin",
                table: "Metas",
                newName: "FechaTermino");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "Metas",
                newName: "CantidadMl");

            migrationBuilder.RenameColumn(
                name: "id_perfil",
                table: "Metas",
                newName: "IdPerfil");

            migrationBuilder.AddColumn<int>(
                name: "IdTipoDeActividad",
                table: "RegistrosDeActFisica",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "Perfiles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeModificacion",
                table: "Perfiles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSyncConGoogleFit",
                table: "Perfiles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdPaisDeResidencia",
                table: "Perfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Metas",
                keyColumn: "Notas",
                keyValue: null,
                column: "Notas",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Metas",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "DatosMedicos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TemaDeColor = table.Column<int>(type: "int", nullable: false),
                    AportaDatosAbiertos = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormulariosRecurrentesActivados = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IntegradoConGoogleFit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreferenciasDeNotificaciones = table.Column<int>(type: "int", nullable: false),
                    IdDispositivo = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoLocalizacion = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdPerfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuracion_Perfiles_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntornosDesbloqueados",
                columns: table => new
                {
                    EntornosDesbloqueadosId = table.Column<int>(type: "int", nullable: false),
                    PerfilesQueDesbloquearonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntornosDesbloqueados", x => new { x.EntornosDesbloqueadosId, x.PerfilesQueDesbloquearonId });
                    table.ForeignKey(
                        name: "FK_EntornosDesbloqueados_Entornos_EntornosDesbloqueadosId",
                        column: x => x.EntornosDesbloqueadosId,
                        principalTable: "Entornos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntornosDesbloqueados_Perfiles_PerfilesQueDesbloquearonId",
                        column: x => x.PerfilesQueDesbloquearonId,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EtiquetasDeMetas",
                columns: table => new
                {
                    EtiquetasId = table.Column<int>(type: "int", nullable: false),
                    EtiquetasIdPerfil = table.Column<int>(type: "int", nullable: false),
                    MetasId = table.Column<int>(type: "int", nullable: false),
                    MetasIdPerfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtiquetasDeMetas", x => new { x.EtiquetasId, x.EtiquetasIdPerfil, x.MetasId, x.MetasIdPerfil });
                    table.ForeignKey(
                        name: "FK_EtiquetasDeMetas_Etiquetas_EtiquetasId_EtiquetasIdPerfil",
                        columns: x => new { x.EtiquetasId, x.EtiquetasIdPerfil },
                        principalTable: "Etiquetas",
                        principalColumns: new[] { "Id", "IdPerfil" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EtiquetasDeMetas_Metas_MetasId_MetasIdPerfil",
                        columns: x => new { x.MetasId, x.MetasIdPerfil },
                        principalTable: "Metas",
                        principalColumns: new[] { "Id", "IdPerfil" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TiposDeActividad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    METs = table.Column<double>(type: "double", nullable: false),
                    VelocidadPromedioKMH = table.Column<double>(type: "double", nullable: false),
                    IdActividadGoogleFit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeActividad", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TokensFCM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TimestampGenerado = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TimestampPersistido = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensFCM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokensFCM_Perfiles_IdPerfil",
                        column: x => x.IdPerfil,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Entornos",
                columns: new[] { "Id", "PrecioEnMonedas", "UrlImagen" },
                values: new object[,]
                {
                    { 1, 0, "1" },
                    { 2, 250, "2" }
                });

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Codigo" },
                values: new object[,]
                {
                    { 1, "--" },
                    { 2, "MX" },
                    { 3, "USA" }
                });

            migrationBuilder.InsertData(
                table: "TiposDeActividad",
                columns: new[] { "Id", "IdActividadGoogleFit", "METs", "VelocidadPromedioKMH" },
                values: new object[,]
                {
                    { 1, 7, 4.3, 5.0 },
                    { 2, 8, 7.0, 8.0 },
                    { 3, 1, 7.5, 11.0 },
                    { 4, 82, 9.8, 0.0 },
                    { 5, 29, 7.0, 0.0 },
                    { 6, 12, 6.5, 0.0 },
                    { 7, 89, 4.0, 0.0 },
                    { 8, 24, 7.8, 0.0 },
                    { 9, 100, 1.3, 0.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RutinasDeActFisica_IdPerfil",
                table: "RutinasDeActFisica",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSemanal_IdPerfil",
                table: "ReporteSemanal",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeHidratacion_IdPerfil",
                table: "RegistrosDeHidratacion",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeActFisica_IdPerfil",
                table: "RegistrosDeActFisica",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeActFisica_IdTipoDeActividad",
                table: "RegistrosDeActFisica",
                column: "IdTipoDeActividad");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_IdEntornoSeleccionado",
                table: "Perfiles",
                column: "IdEntornoSeleccionado");

            migrationBuilder.CreateIndex(
                name: "IX_Perfiles_IdPaisDeResidencia",
                table: "Perfiles",
                column: "IdPaisDeResidencia");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_IdPerfil",
                table: "Metas",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_IdPerfil",
                table: "Etiquetas",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_DatosMedicos_IdPerfil",
                table: "DatosMedicos",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_IdPerfil",
                table: "Configuracion",
                column: "IdPerfil",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntornosDesbloqueados_PerfilesQueDesbloquearonId",
                table: "EntornosDesbloqueados",
                column: "PerfilesQueDesbloquearonId");

            migrationBuilder.CreateIndex(
                name: "IX_EtiquetasDeMetas_MetasId_MetasIdPerfil",
                table: "EtiquetasDeMetas",
                columns: new[] { "MetasId", "MetasIdPerfil" });

            migrationBuilder.CreateIndex(
                name: "IX_TokensFCM_IdPerfil",
                table: "TokensFCM",
                column: "IdPerfil",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DatosMedicos_Perfiles_IdPerfil",
                table: "DatosMedicos",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Etiquetas_Perfiles_IdPerfil",
                table: "Etiquetas",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Metas_Perfiles_IdPerfil",
                table: "Metas",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Entornos_IdEntornoSeleccionado",
                table: "Perfiles",
                column: "IdEntornoSeleccionado",
                principalTable: "Entornos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Paises_IdPaisDeResidencia",
                table: "Perfiles",
                column: "IdPaisDeResidencia",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeActFisica_Perfiles_IdPerfil",
                table: "RegistrosDeActFisica",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeActFisica_TiposDeActividad_IdTipoDeActividad",
                table: "RegistrosDeActFisica",
                column: "IdTipoDeActividad",
                principalTable: "TiposDeActividad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_IdPerfil",
                table: "RegistrosDeHidratacion",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSemanal_Perfiles_IdPerfil",
                table: "ReporteSemanal",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RutinasDeActFisica_Perfiles_IdPerfil",
                table: "RutinasDeActFisica",
                column: "IdPerfil",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RutinasDeActFisica_RegistrosDeActFisica_IdRegistroActividad_~",
                table: "RutinasDeActFisica",
                columns: new[] { "IdRegistroActividad", "IdPerfil" },
                principalTable: "RegistrosDeActFisica",
                principalColumns: new[] { "Id", "IdPerfil" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatosMedicos_Perfiles_IdPerfil",
                table: "DatosMedicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Etiquetas_Perfiles_IdPerfil",
                table: "Etiquetas");

            migrationBuilder.DropForeignKey(
                name: "FK_Metas_Perfiles_IdPerfil",
                table: "Metas");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Entornos_IdEntornoSeleccionado",
                table: "Perfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfiles_Paises_IdPaisDeResidencia",
                table: "Perfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeActFisica_Perfiles_IdPerfil",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeActFisica_TiposDeActividad_IdTipoDeActividad",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_IdPerfil",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropForeignKey(
                name: "FK_ReporteSemanal_Perfiles_IdPerfil",
                table: "ReporteSemanal");

            migrationBuilder.DropForeignKey(
                name: "FK_RutinasDeActFisica_Perfiles_IdPerfil",
                table: "RutinasDeActFisica");

            migrationBuilder.DropForeignKey(
                name: "FK_RutinasDeActFisica_RegistrosDeActFisica_IdRegistroActividad_~",
                table: "RutinasDeActFisica");

            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "EntornosDesbloqueados");

            migrationBuilder.DropTable(
                name: "EtiquetasDeMetas");

            migrationBuilder.DropTable(
                name: "TiposDeActividad");

            migrationBuilder.DropTable(
                name: "TokensFCM");

            migrationBuilder.DropIndex(
                name: "IX_RutinasDeActFisica_IdPerfil",
                table: "RutinasDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_ReporteSemanal_IdPerfil",
                table: "ReporteSemanal");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeHidratacion_IdPerfil",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeActFisica_IdPerfil",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosDeActFisica_IdTipoDeActividad",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropIndex(
                name: "IX_Perfiles_IdEntornoSeleccionado",
                table: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Perfiles_IdPaisDeResidencia",
                table: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Metas_IdPerfil",
                table: "Metas");

            migrationBuilder.DropIndex(
                name: "IX_Etiquetas_IdPerfil",
                table: "Etiquetas");

            migrationBuilder.DropIndex(
                name: "IX_DatosMedicos_IdPerfil",
                table: "DatosMedicos");

            migrationBuilder.DeleteData(
                table: "Entornos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Entornos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IdTipoDeActividad",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "FechaDeModificacion",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "FechaSyncConGoogleFit",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "IdPaisDeResidencia",
                table: "Perfiles");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "DatosMedicos");

            migrationBuilder.RenameColumn(
                name: "IdRegistroActividad",
                table: "RutinasDeActFisica",
                newName: "id_actividad");

            migrationBuilder.RenameColumn(
                name: "IdPerfil",
                table: "RutinasDeActFisica",
                newName: "id_perfil");

            migrationBuilder.RenameColumn(
                name: "DiasDeOcurrencia",
                table: "RutinasDeActFisica",
                newName: "Dias");

            migrationBuilder.RenameIndex(
                name: "IX_RutinasDeActFisica_IdRegistroActividad_IdPerfil",
                table: "RutinasDeActFisica",
                newName: "IX_RutinasDeActFisica_id_actividad_id_perfil");

            migrationBuilder.RenameColumn(
                name: "IdPerfil",
                table: "RegistrosDeHidratacion",
                newName: "id_perfil");

            migrationBuilder.RenameColumn(
                name: "RecomensaMonedas",
                table: "Metas",
                newName: "recompensa");

            migrationBuilder.RenameColumn(
                name: "FechaTermino",
                table: "Metas",
                newName: "fecha_fin");

            migrationBuilder.RenameColumn(
                name: "CantidadMl",
                table: "Metas",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "IdPerfil",
                table: "Metas",
                newName: "id_perfil");

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "RutinasDeActFisica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "ReporteSemanal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DatosActividadId",
                table: "RegistrosDeActFisica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "RegistrosDeActFisica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisDeResidenciaId",
                table: "Perfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Metas",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "Metas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "Etiquetas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Entornos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilDeUsuarioId",
                table: "DatosMedicos",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_RutinasDeActFisica_PerfilDeUsuarioId",
                table: "RutinasDeActFisica",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSemanal_PerfilDeUsuarioId",
                table: "ReporteSemanal",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeHidratacion_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
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
                name: "IX_Perfiles_PaisDeResidenciaId",
                table: "Perfiles",
                column: "PaisDeResidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_PerfilDeUsuarioId",
                table: "Metas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_PerfilDeUsuarioId",
                table: "Etiquetas",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Entornos_PerfilId",
                table: "Entornos",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosMedicos_PerfilDeUsuarioId",
                table: "DatosMedicos",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_De_Meta_MetasId_MetasIdPerfil",
                table: "Etiquetas_De_Meta",
                columns: new[] { "MetasId", "MetasIdPerfil" });

            migrationBuilder.AddForeignKey(
                name: "FK_DatosMedicos_Perfiles_PerfilDeUsuarioId",
                table: "DatosMedicos",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entornos_Perfiles_PerfilId",
                table: "Entornos",
                column: "PerfilId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Etiquetas_Perfiles_PerfilDeUsuarioId",
                table: "Etiquetas",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metas_Perfiles_PerfilDeUsuarioId",
                table: "Metas",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfiles_Paises_PaisDeResidenciaId",
                table: "Perfiles",
                column: "PaisDeResidenciaId",
                principalTable: "Paises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeActFisica_DatosDeActividades_DatosActividadId",
                table: "RegistrosDeActFisica",
                column: "DatosActividadId",
                principalTable: "DatosDeActividades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeActFisica_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeActFisica",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosDeHidratacion_Perfiles_PerfilDeUsuarioId",
                table: "RegistrosDeHidratacion",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReporteSemanal_Perfiles_PerfilDeUsuarioId",
                table: "ReporteSemanal",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RutinasDeActFisica_Perfiles_PerfilDeUsuarioId",
                table: "RutinasDeActFisica",
                column: "PerfilDeUsuarioId",
                principalTable: "Perfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RutinasDeActFisica_RegistrosDeActFisica_id_actividad_id_perf~",
                table: "RutinasDeActFisica",
                columns: new[] { "id_actividad", "id_perfil" },
                principalTable: "RegistrosDeActFisica",
                principalColumns: new[] { "Id", "IdPerfil" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
