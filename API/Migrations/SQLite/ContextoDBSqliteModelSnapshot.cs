﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicioHydrate.Data;

#nullable disable

namespace ServicioHydrate.Migrations.SQLite
{
    [DbContext(typeof(ContextoDBSqlite))]
    partial class ContextoDBSqliteModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("ComentarioUsuario", b =>
                {
                    b.Property<int>("ComentariosReportadosId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ReportesDeUsuariosId")
                        .HasColumnType("TEXT");

                    b.HasKey("ComentariosReportadosId", "ReportesDeUsuariosId");

                    b.HasIndex("ReportesDeUsuariosId");

                    b.ToTable("ComentariosReportados", (string)null);
                });

            modelBuilder.Entity("ComentarioUsuario1", b =>
                {
                    b.Property<int>("ComentariosUtilesId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UtilParaUsuariosId")
                        .HasColumnType("TEXT");

                    b.HasKey("ComentariosUtilesId", "UtilParaUsuariosId");

                    b.HasIndex("UtilParaUsuariosId");

                    b.ToTable("ComentariosUtiles", (string)null);
                });

            modelBuilder.Entity("EtiquetaMeta", b =>
                {
                    b.Property<int>("EtiquetasId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EtiquetasIdPerfil")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MetasId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MetasIdPerfil")
                        .HasColumnType("INTEGER");

                    b.HasKey("EtiquetasId", "EtiquetasIdPerfil", "MetasId", "MetasIdPerfil");

                    b.HasIndex("MetasId", "MetasIdPerfil");

                    b.ToTable("Etiquetas_De_Meta", (string)null);
                });

            modelBuilder.Entity("RespuestaUsuario", b =>
                {
                    b.Property<int>("RespuestasUtilesId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UtilParaUsuariosId")
                        .HasColumnType("TEXT");

                    b.HasKey("RespuestasUtilesId", "UtilParaUsuariosId");

                    b.HasIndex("UtilParaUsuariosId");

                    b.ToTable("RespuestasUtiles", (string)null);
                });

            modelBuilder.Entity("RespuestaUsuario1", b =>
                {
                    b.Property<Guid>("ReportesDeUsuariosId")
                        .HasColumnType("TEXT");

                    b.Property<int>("RespuestasReportadasId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReportesDeUsuariosId", "RespuestasReportadasId");

                    b.HasIndex("RespuestasReportadasId");

                    b.ToTable("RespuestasReportadas", (string)null);
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Asunto")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AutorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Fecha")
                        .HasMaxLength(33)
                        .HasColumnType("TEXT");

                    b.Property<bool>("NecesitaModificaciones")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Publicado")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.ComentarioArchivado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fecha")
                        .HasMaxLength(33)
                        .HasColumnType("TEXT");

                    b.Property<int>("IdComentario")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Motivo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ComentariosArchivados");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.ActividadFisica", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DatosActividadId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Distancia")
                        .HasColumnType("REAL");

                    b.Property<int>("Duracion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FueAlAireLibre")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KcalQuemadas")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("DatosActividadId");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("RegistrosDeActFisica");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.DatosDeActividad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("METs")
                        .HasColumnType("REAL");

                    b.Property<double>("VelocidadPromedioKMH")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("DatosDeActividades");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.DatosMedicos", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER");

                    b.Property<float>("AguaExtracelular")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("FechaProxCita")
                        .HasColumnType("TEXT");

                    b.Property<float>("GananciaReal")
                        .HasColumnType("REAL");

                    b.Property<float>("GananciaRegistrada")
                        .HasColumnType("REAL");

                    b.Property<float>("Hipervolemia")
                        .HasColumnType("REAL");

                    b.Property<float>("Normovolemia")
                        .HasColumnType("REAL");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("PesoPostDialisis")
                        .HasColumnType("REAL");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("DatosMedicos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Etiqueta", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Valor")
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("Etiquetas");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.HabitosSemanales", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<double>("HorasDeActividadFisica")
                        .HasColumnType("REAL");

                    b.Property<double>("HorasDeOcupacion")
                        .HasColumnType("REAL");

                    b.Property<double>("HorasDeSuenio")
                        .HasColumnType("REAL");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TemperaturaMaxima")
                        .HasColumnType("REAL");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("HabitosSemanales");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Meta", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_perfil");

                    b.Property<int>("CantidadEnMl")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cantidad");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaTermino")
                        .HasColumnType("TEXT")
                        .HasColumnName("fecha_fin");

                    b.Property<string>("Notas")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Plazo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecompensaDeMonedas")
                        .HasColumnType("INTEGER")
                        .HasColumnName("recompensa");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("Metas");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.RegistroDeHidratacion", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_perfil");

                    b.Property<int>("CantidadEnMl")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PorcentajeCargaBateria")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TemperaturaAproximada")
                        .HasColumnType("REAL");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.ToTable("RegistrosDeHidratacion");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Rutina", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_perfil");

                    b.Property<int>("Dias")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("Hora")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdActividad")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_actividad");

                    b.Property<int?>("PerfilDeUsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id", "IdPerfil");

                    b.HasIndex("PerfilDeUsuarioId");

                    b.HasIndex("IdActividad", "IdPerfil")
                        .IsUnique();

                    b.ToTable("RutinasDeActFisica");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Entorno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PerfilId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PrecioEnMonedas")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UrlImagen")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PerfilId");

                    b.ToTable("Entornos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Orden", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ClienteId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Estado")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fecha")
                        .HasMaxLength(33)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Ordenes");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Pais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Paises");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Perfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellido")
                        .HasColumnType("TEXT");

                    b.Property<int>("CantidadMonedas")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CondicionMedica")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Estatura")
                        .HasColumnType("REAL");

                    b.Property<string>("FechaNacimiento")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IdCuentaUsuario")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdEntornoSeleccionado")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumModificaciones")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ocupacion")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PaisDeResidenciaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Peso")
                        .HasColumnType("REAL");

                    b.Property<int>("SexoUsuario")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IdCuentaUsuario")
                        .IsUnique();

                    b.HasIndex("PaisDeResidenciaId");

                    b.ToTable("Perfiles");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<int>("Disponibles")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("TEXT");

                    b.Property<string>("UrlImagen")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.ProductosOrdenados", b =>
                {
                    b.Property<Guid>("IdOrden")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdProducto")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cantidad")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdOrden", "IdProducto");

                    b.HasIndex("IdProducto");

                    b.ToTable("ProductosOrdenados");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.RecursoInformativo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("FechaPublicacion")
                        .HasMaxLength(33)
                        .HasColumnType("TEXT");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Recursos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Respuesta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("AutorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Fecha")
                        .HasMaxLength(33)
                        .HasColumnType("TEXT");

                    b.Property<int>("IdComentario")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Publicado")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("IdComentario");

                    b.ToTable("Respuestas");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<int>("RolDeUsuario")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ComentarioUsuario", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Comentario", null)
                        .WithMany()
                        .HasForeignKey("ComentariosReportadosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Usuario", null)
                        .WithMany()
                        .HasForeignKey("ReportesDeUsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ComentarioUsuario1", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Comentario", null)
                        .WithMany()
                        .HasForeignKey("ComentariosUtilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UtilParaUsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EtiquetaMeta", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Datos.Etiqueta", null)
                        .WithMany()
                        .HasForeignKey("EtiquetasId", "EtiquetasIdPerfil")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Datos.Meta", null)
                        .WithMany()
                        .HasForeignKey("MetasId", "MetasIdPerfil")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RespuestaUsuario", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Respuesta", null)
                        .WithMany()
                        .HasForeignKey("RespuestasUtilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UtilParaUsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RespuestaUsuario1", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", null)
                        .WithMany()
                        .HasForeignKey("ReportesDeUsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Respuesta", null)
                        .WithMany()
                        .HasForeignKey("RespuestasReportadasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Comentario", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", "Autor")
                        .WithMany("Comentarios")
                        .HasForeignKey("AutorId");

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.ActividadFisica", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Datos.DatosDeActividad", "DatosActividad")
                        .WithMany()
                        .HasForeignKey("DatosActividadId");

                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("RegistrosDeActFisica")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("DatosActividad");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.DatosMedicos", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("RegistrosMedicos")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Etiqueta", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("Etiquetas")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.HabitosSemanales", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("ReportesSemanales")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Meta", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("Metas")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.RegistroDeHidratacion", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("RegistrosDeHidratacion")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.Navigation("PerfilDeUsuario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.Rutina", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", "PerfilDeUsuario")
                        .WithMany("Rutinas")
                        .HasForeignKey("PerfilDeUsuarioId");

                    b.HasOne("ServicioHydrate.Modelos.Datos.ActividadFisica", "RegistroDeActividad")
                        .WithOne("Rutina")
                        .HasForeignKey("ServicioHydrate.Modelos.Datos.Rutina", "IdActividad", "IdPerfil")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PerfilDeUsuario");

                    b.Navigation("RegistroDeActividad");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Entorno", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Perfil", null)
                        .WithMany("EntornosDesbloqueados")
                        .HasForeignKey("PerfilId");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Orden", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", "Cliente")
                        .WithMany("Ordenes")
                        .HasForeignKey("ClienteId");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Perfil", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", "Cuenta")
                        .WithOne("PerfilDeUsuario")
                        .HasForeignKey("ServicioHydrate.Modelos.Perfil", "IdCuentaUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Pais", "PaisDeResidencia")
                        .WithMany("Perfiles")
                        .HasForeignKey("PaisDeResidenciaId");

                    b.Navigation("Cuenta");

                    b.Navigation("PaisDeResidencia");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.ProductosOrdenados", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Orden", "Orden")
                        .WithMany("Productos")
                        .HasForeignKey("IdOrden")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioHydrate.Modelos.Producto", "Producto")
                        .WithMany("OrdenesDelProducto")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orden");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Respuesta", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", "Autor")
                        .WithMany("Respuestas")
                        .HasForeignKey("AutorId");

                    b.HasOne("ServicioHydrate.Modelos.Comentario", "Comentario")
                        .WithMany("Respuestas")
                        .HasForeignKey("IdComentario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Comentario");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Comentario", b =>
                {
                    b.Navigation("Respuestas");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Datos.ActividadFisica", b =>
                {
                    b.Navigation("Rutina");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Orden", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Pais", b =>
                {
                    b.Navigation("Perfiles");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Perfil", b =>
                {
                    b.Navigation("EntornosDesbloqueados");

                    b.Navigation("Etiquetas");

                    b.Navigation("Metas");

                    b.Navigation("RegistrosDeActFisica");

                    b.Navigation("RegistrosDeHidratacion");

                    b.Navigation("RegistrosMedicos");

                    b.Navigation("ReportesSemanales");

                    b.Navigation("Rutinas");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Producto", b =>
                {
                    b.Navigation("OrdenesDelProducto");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Usuario", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Ordenes");

                    b.Navigation("PerfilDeUsuario");

                    b.Navigation("Respuestas");
                });
#pragma warning restore 612, 618
        }
    }
}
