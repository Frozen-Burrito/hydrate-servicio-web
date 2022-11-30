using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Data 
{
    /// Permite la interacción con la base de datos en MySQL y las 
    /// entidades de EF core. 
    /// Se utilizan dos contextos de DB diferentes para administrar las 
    /// migraciones de los proveedores especificos.  
    public class ContextoDBMysql : DbContext
    {
        public ContextoDBMysql(DbContextOptions<ContextoDBMysql> opciones)
            : base(opciones)
        {
        }

        /// Colección de entidades de Usuario.
        public DbSet<Usuario> Usuarios { get; set; }

        /// Colección de entidades de RecursoInformativo.
        public DbSet<RecursoInformativo> Recursos { get; set; }

        // Colección de entidades de Comentario.
        public DbSet<Comentario> Comentarios { get; set; }

        // Colección de entidades de Respuesta.
        public DbSet<Respuesta> Respuestas { get; set; }

        // Colección de entidades de Orden.
        public DbSet<Orden> Ordenes { get; set; }

        // Colección de entidades de Producto.
        public DbSet<Producto> Productos { get; set; }
    
        public DbSet<ComentarioArchivado> ComentariosArchivados { get; set; }

        public DbSet<Perfil> Perfiles { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Entorno> Entornos { get; set; }

        public DbSet<RegistroDeActividad> RegistrosDeActFisica { get; set; } 

        public DbSet<TipoDeActividad> DatosDeActividades { get; set; }

        public DbSet<DatosMedicos> DatosMedicos { get; set; }

        public DbSet<MetaHidratacion> Metas { get; set; }

        public DbSet<Etiqueta> Etiquetas { get; set; }

        public DbSet<RegistroDeHidratacion> RegistrosDeHidratacion { get; set; }

        public DbSet<Rutina> RutinasDeActFisica { get; set; }

        public DbSet<LlaveDeApi> LlavesDeAPI { get; set; }

        public DbSet<Configuracion> Configuraciones { get; set; }

        public DbSet<TokenFCM> TokensParaNotificaciones { get; set; }

        /// Configura la creación de cada entidad en la base de datos. (No la inserción)
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasColumnType("char(36)");

            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);

            // Relacion uno a muchos entre Usuario y Comentarios.
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Comentarios)
                .WithOne(c => c.Autor);

            // Relacion uno a muchos entre Usuario y Respuestas.
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Respuestas)
                .WithOne(r => r.Autor);

            // Configurar relacion muchos a muchos entre usuarios y comentarios 
            // para los comentarios reportados.
            modelBuilder.Entity<Comentario>()
                .HasMany(c => c.ReportesDeUsuarios)
                .WithMany(u => u.ComentariosReportados)
                .UsingEntity(j => j.ToTable("ComentariosReportados"));

            // Configurar relacion muchos a muchos entre usuarios y comentarios 
            // para los comentarios marcados como utiles.
            modelBuilder.Entity<Comentario>()
                .HasMany(c => c.UtilParaUsuarios)
                .WithMany(u => u.ComentariosUtiles)
                .UsingEntity(j => j.ToTable("ComentariosUtiles"));

            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.Comentario)
                .WithMany(c => c.Respuestas)
                .HasForeignKey(r => r.IdComentario);

            modelBuilder.Entity<Respuesta>()
                .HasMany(r => r.UtilParaUsuarios)
                .WithMany(u => u.RespuestasUtiles)
                .UsingEntity(j => j.ToTable("RespuestasUtiles"));

            modelBuilder.Entity<Respuesta>()
                .HasMany(r => r.ReportesDeUsuarios)
                .WithMany(u => u.RespuestasReportadas)
                .UsingEntity(j => j.ToTable("RespuestasReportadas"));

            modelBuilder.Entity<Orden>()
                .Property(o => o.Id)
                .HasColumnType("char(36)");

            // Relación uno a muchos entre Usuario y Orden
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Ordenes)
                .WithOne(o => o.Cliente);

            modelBuilder.Entity<ProductosOrdenados>()
                .HasKey(po => new { po.IdOrden, po.IdProducto });

            // Relación muchos a muchos entre Orden y Producto.
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.Productos)
                .WithOne(po => po.Orden)
                .HasForeignKey(po => po.IdOrden);

            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioUnitario)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.OrdenesDelProducto)
                .WithOne(po => po.Producto)
                .HasForeignKey(po => po.IdProducto);

            // Relación uno a muchos entre Pais y Perfil.
            modelBuilder.Entity<Pais>()
                .HasMany(pa => pa.PerfilesQueResidenEnPais)
                .WithOne(pe => pe.PaisDeResidencia)
                .HasForeignKey(pe => pe.IdPaisDeResidencia);

            modelBuilder.Entity<Perfil>()
                .HasOne(p => p.Cuenta)
                .WithOne(u => u.PerfilDeUsuario)
                .HasForeignKey<Perfil>(p => p.IdCuentaUsuario);

            // Definir llaves primarias compuestas para todas las 
            // entidades asociadas con un Perfil. 
            // (Esto es para identificar los registros de distintos usuarios.)
            modelBuilder.Entity<RegistroDeActividad>()
                .HasKey(af => new { af.Id, af.IdPerfil });
                
            modelBuilder.Entity<DatosMedicos>()
                .HasKey(dm => new { dm.Id, dm.IdPerfil });

            modelBuilder.Entity<Etiqueta>()
                .HasKey(e => new { e.Id, e.IdPerfil });

            modelBuilder.Entity<ReporteSemanal>()
                .HasKey(hs => new { hs.Id, hs.IdPerfil });

            modelBuilder.Entity<MetaHidratacion>()
                .HasKey(m => new { m.Id, m.IdPerfil });

            modelBuilder.Entity<RegistroDeHidratacion>()
                .HasKey(rh => new { rh.Id, idPerfil = rh.IdPerfil });

            modelBuilder.Entity<Rutina>()
                .HasKey(ru => new { ru.Id, ru.IdPerfil });

            // Determinar las relaciones entre entidades de datos de perfil.
            modelBuilder.Entity<RegistroDeActividad>()
                .HasOne(ra => ra.Perfil)
                .WithMany(p => p.RegistrosDeActFisica)
                .HasForeignKey(ra => ra.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<RegistroDeActividad>()
                .HasOne(ra => ra.TipoDeActividad)
                .WithMany(ta => ta.RegistrosDeActividad)
                .HasForeignKey(ra => ra.IdTipoDeActividad)
                .IsRequired();
                
            modelBuilder.Entity<DatosMedicos>()
                .HasOne(dm => dm.PerfilDeUsuario)
                .WithMany(p => p.RegistrosMedicos)
                .HasForeignKey(dm => dm.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<Etiqueta>()
                .HasOne(e => e.Perfil)
                .WithMany(p => p.Etiquetas)
                .HasForeignKey(e => e.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<ReporteSemanal>()
                .HasOne(hs => hs.Perfil)
                .WithMany(p => p.ReportesSemanales)
                .HasForeignKey(dm => dm.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<MetaHidratacion>()
                .HasOne(m => m.Perfil)
                .WithMany(p => p.Metas)
                .HasForeignKey(m => m.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<RegistroDeHidratacion>()
                .HasOne(rh => rh.Perfil)
                .WithMany(p => p.RegistrosDeHidratacion)
                .HasForeignKey(rh => rh.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<Rutina>()
                .HasOne(r => r.Perfil)
                .WithMany(p => p.Rutinas)
                .HasForeignKey(r => r.IdPerfil)
                .IsRequired();
            
            // Relacion muchos-a-muchos entre Meta y Etiqueta.
            modelBuilder.Entity<MetaHidratacion>()
                .HasMany(m => m.Etiquetas)
                .WithMany(e => e.Metas)
                .UsingEntity(j => j.ToTable("EtiquetasDeMetas"));

            modelBuilder.Entity<Entorno>()
                .HasMany(e => e.PerfilesQueDesbloquearon)
                .WithMany(p => p.EntornosDesbloqueados)
                .UsingEntity(j => j.ToTable("EntornosDesbloqueados"));

            modelBuilder.Entity<Entorno>()
                .HasMany(e => e.PerfilesQueSeleccionaron)
                .WithOne(p => p.EntornoSeleccionado)
                .HasForeignKey(p => p.IdEntornoSeleccionado);

            // Seeding de entornos, paises y tipos de actividad fisica.
            modelBuilder.Entity<Entorno>()
                .HasData(new List<Entorno>() {
                    Entorno.PrimerEntornoDesbloqueado,
                    new Entorno { Id = 2, UrlImagen = "2", PrecioEnMonedas = 250 },
                });

            modelBuilder.Entity<Pais>()
                .HasData(new List<Pais>() {
                    Pais.PaisNoEspecificado,
                    new Pais { Id = 2, Codigo = "MX" },
                    new Pais { Id = 3, Codigo = "USA" },
                });

            modelBuilder.Entity<TipoDeActividad>() 
                .HasData(new List<TipoDeActividad>() {
                    new TipoDeActividad { Id = 1, METs = 4.3, VelocidadPromedioKMH = 5.0, IdActividadGoogleFit = 7 },
                    new TipoDeActividad { Id = 2, METs = 7.0, VelocidadPromedioKMH = 8.0, IdActividadGoogleFit = 8 },
                    new TipoDeActividad { Id = 3, METs = 7.5, VelocidadPromedioKMH = 11.0, IdActividadGoogleFit = 1 },
                    new TipoDeActividad { Id = 4, METs = 9.8, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 82 },
                    new TipoDeActividad { Id = 5, METs = 7.0, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 29 },
                    new TipoDeActividad { Id = 6, METs = 6.5, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 12 },
                    new TipoDeActividad { Id = 7, METs = 4.0, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 89 },
                    new TipoDeActividad { Id = 8, METs = 7.8, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 24 },
                    new TipoDeActividad { Id = 9, METs = 1.3, VelocidadPromedioKMH = 0.0, IdActividadGoogleFit = 100 },
                });

            // Relacion uno-a-uno entre ActividadFisica y Rutina.
            modelBuilder.Entity<Rutina>()
                .HasOne(r => r.RegistroDeActividad)
                .WithOne(af => af.Rutina)
                .HasForeignKey<Rutina>(r => new { Id = r.IdActividad, r.IdPerfil })
                .IsRequired();
                
            // Relación uno a muchos entre Usuario y LlaveDeAPI
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.LlavesDeAPI)
                .WithOne(ll => ll.Usuario)
                .HasForeignKey(ll => ll.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<Configuracion>()
                .HasOne(c => c.Perfil)
                .WithOne(p => p.Configuracion)
                .HasForeignKey<Configuracion>(c => c.IdPerfil)
                .IsRequired();

            modelBuilder.Entity<TokenFCM>()
                .HasOne(t => t.Perfil)
                .WithOne(p => p.TokenFCM)
                .HasForeignKey<TokenFCM>(t => t.IdPerfil)
                .IsRequired();
        }
    }
}