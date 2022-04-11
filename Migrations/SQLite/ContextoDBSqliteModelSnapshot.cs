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

                    b.Property<bool>("Publicado")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.ToTable("Comentarios");
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

            modelBuilder.Entity("ServicioHydrate.Modelos.Orden", b =>
                {
                    b.HasOne("ServicioHydrate.Modelos.Usuario", "Cliente")
                        .WithMany("Ordenes")
                        .HasForeignKey("ClienteId");

                    b.Navigation("Cliente");
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

            modelBuilder.Entity("ServicioHydrate.Modelos.Orden", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Producto", b =>
                {
                    b.Navigation("OrdenesDelProducto");
                });

            modelBuilder.Entity("ServicioHydrate.Modelos.Usuario", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Ordenes");

                    b.Navigation("Respuestas");
                });
#pragma warning restore 612, 618
        }
    }
}
