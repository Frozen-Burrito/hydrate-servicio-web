using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    public partial class AgregarConfiguracionYTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitosSemanales");

            migrationBuilder.DropColumn(
                name: "Dias",
                table: "RutinasDeActFisica");

            migrationBuilder.AddColumn<bool>(
                name: "EsInformacionAbierta",
                table: "RegistrosDeHidratacion",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "RegistrosDeActFisica",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "RegistrosDeActFisica",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsInformacionAbierta",
                table: "RegistrosDeActFisica",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LlavesDeAPI",
                table: "LlavesDeAPI",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TemaDeColor = table.Column<int>(type: "INTEGER", nullable: false),
                    AportaDatosAbiertos = table.Column<bool>(type: "INTEGER", nullable: false),
                    FormulariosRecurrentesActivados = table.Column<bool>(type: "INTEGER", nullable: false),
                    IntegradoConGoogleFit = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotificacionesPermitidas = table.Column<int>(type: "INTEGER", nullable: false),
                    IdDispositivo = table.Column<string>(type: "TEXT", maxLength: 17, nullable: true),
                    CodigoLocalizacion = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    id_perfil = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuracion_Perfiles_id_perfil",
                        column: x => x.id_perfil,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReporteSemanal",
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
                    table.PrimaryKey("PK_ReporteSemanal", x => new { x.Id, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_ReporteSemanal_Perfiles_PerfilDeUsuarioId",
                        column: x => x.PerfilDeUsuarioId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TokensFCM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    TimestampGenerado = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimestampPersistido = table.Column<DateTime>(type: "TEXT", nullable: false),
                    id_perfil = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensFCM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokensFCM_Perfiles_id_perfil",
                        column: x => x.id_perfil,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_id_perfil",
                table: "Configuracion",
                column: "id_perfil",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReporteSemanal_PerfilDeUsuarioId",
                table: "ReporteSemanal",
                column: "PerfilDeUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TokensFCM_id_perfil",
                table: "TokensFCM",
                column: "id_perfil",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "ReporteSemanal");

            migrationBuilder.DropTable(
                name: "TokensFCM");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LlavesDeAPI",
                table: "LlavesDeAPI");

            migrationBuilder.DropColumn(
                name: "EsInformacionAbierta",
                table: "RegistrosDeHidratacion");

            migrationBuilder.DropColumn(
                name: "EsInformacionAbierta",
                table: "RegistrosDeActFisica");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LlavesDeAPI");

            migrationBuilder.AddColumn<int>(
                name: "Dias",
                table: "RutinasDeActFisica",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "RegistrosDeActFisica",
                type: "TEXT",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "Fecha",
                table: "RegistrosDeActFisica",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "HabitosSemanales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPerfil = table.Column<int>(type: "INTEGER", nullable: false),
                    PerfilDeUsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HorasDeActividadFisica = table.Column<double>(type: "REAL", nullable: false),
                    HorasDeOcupacion = table.Column<double>(type: "REAL", nullable: false),
                    HorasDeSuenio = table.Column<double>(type: "REAL", nullable: false),
                    TemperaturaMaxima = table.Column<double>(type: "REAL", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_HabitosSemanales_PerfilDeUsuarioId",
                table: "HabitosSemanales",
                column: "PerfilDeUsuarioId");
        }
    }
}
