using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Data 
{
    /// Permite la interacción con la base de datos en SQLite y las 
    /// entidades de EF core. 
    /// Se utilizan dos contextos de DB diferentes para administrar las 
    /// migraciones de los proveedores específicos.  
    public class ContextoDBSqlite : DbContext
    {
        public ContextoDBSqlite(DbContextOptions<ContextoDBSqlite> opciones)
            : base(opciones)
        {
        }

        // DbSets para cada una de las entidades de la BD, para que 
        // puedan ser usadas a través de este contexto.
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<RecursoInformativo> Recursos { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<Respuesta> Respuestas { get; set; }

        public DbSet<Orden> Ordenes { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<ComentarioArchivado> ComentariosArchivados { get; set; }

        public DbSet<Perfil> Perfiles { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Entorno> Entornos { get; set; }

        public DbSet<ActividadFisica> RegistrosDeActFisica { get; set; } 

        public DbSet<DatosDeActividad> DatosDeActividades { get; set; }

        public DbSet<DatosMedicos> DatosMedicos { get; set; }

        public DbSet<Meta> Metas { get; set; }

        public DbSet<Etiqueta> Etiquetas { get; set; }

        public DbSet<RegistroDeHidratacion> RegistrosDeHidratacion { get; set; }

        public DbSet<Rutina> RutinasDeActFisica { get; set; }
        
        public DbSet<LlaveDeApi> LlavesDeAPI { get; set; }
        
        /// Configura la creación de cada entidad en la base de datos. (No la inserción)
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
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

            // Configurar relación muchos a muchos entre usuarios y comentarios 
            // para los comentarios reportados.
            modelBuilder.Entity<Comentario>()
                .HasMany(c => c.ReportesDeUsuarios)
                .WithMany(u => u.ComentariosReportados)
                .UsingEntity(j => j.ToTable("ComentariosReportados"));

            // Configurar relación muchos a muchos entre usuarios y comentarios 
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
                .HasMany(p => p.OrdenesDelProducto)
                .WithOne(po => po.Producto)
                .HasForeignKey(po => po.IdProducto);

            // Relación uno a muchos entre Pais y Perfil.
            modelBuilder.Entity<Pais>()
                .HasMany(pa => pa.Perfiles)
                .WithOne(pe => pe.PaisDeResidencia);

            modelBuilder.Entity<Perfil>()
                .HasOne(p => p.Cuenta)
                .WithOne(u => u.PerfilDeUsuario)
                .HasForeignKey<Perfil>(p => p.IdCuentaUsuario);

            // Definir llaves primarias compuestas para todas las 
            // entidades asociadas con un Perfil. 
            // (Esto es para identificar los registros de distintos usuarios.)
            modelBuilder.Entity<ActividadFisica>()
                .HasKey(af => new { af.Id, af.IdPerfil });
                
            modelBuilder.Entity<DatosMedicos>()
                .HasKey(dm => new { dm.Id, dm.IdPerfil });

            modelBuilder.Entity<Etiqueta>()
                .HasKey(e => new { e.Id, e.IdPerfil });

            modelBuilder.Entity<ReporteSemanal>()
                .HasKey(hs => new { hs.Id, hs.IdPerfil });

            modelBuilder.Entity<Meta>()
                .HasKey(m => new { m.Id, m.IdPerfil });

            modelBuilder.Entity<RegistroDeHidratacion>()
                .HasKey(rh => new { rh.Id, rh.IdPerfil });

            modelBuilder.Entity<Rutina>()
                .HasKey(ru => new { ru.Id, ru.IdPerfil });

            // Determinar las relaciones entre entidades de datos de perfil.
            modelBuilder.Entity<ActividadFisica>()
                .HasOne(af => af.PerfilDeUsuario)
                .WithMany(p => p.RegistrosDeActFisica);
                
            modelBuilder.Entity<DatosMedicos>()
                .HasOne(dm => dm.PerfilDeUsuario)
                .WithMany(p => p.RegistrosMedicos);

            modelBuilder.Entity<Etiqueta>()
                .HasOne(e => e.PerfilDeUsuario)
                .WithMany(p => p.Etiquetas);

            modelBuilder.Entity<ReporteSemanal>()
                .HasOne(hs => hs.PerfilDeUsuario)
                .WithMany(p => p.ReportesSemanales);

            modelBuilder.Entity<Meta>()
                .HasOne(m => m.PerfilDeUsuario)
                .WithMany(p => p.Metas);

            modelBuilder.Entity<RegistroDeHidratacion>()
                .HasOne(rh => rh.PerfilDeUsuario)
                .WithMany(p => p.RegistrosDeHidratacion);

            modelBuilder.Entity<Rutina>()
                .HasOne(r => r.PerfilDeUsuario)
                .WithMany(p => p.Rutinas);
            
            // Relacion muchos-a-muchos entre Meta y Etiqueta.
            modelBuilder.Entity<Meta>()
                .HasMany(m => m.Etiquetas)
                .WithMany(e => e.Metas)
                .UsingEntity(j => j.ToTable("Etiquetas_De_Meta"));

            // Relacion uno-a-uno entre ActividadFisica y Rutina.
            modelBuilder.Entity<Rutina>()
                .HasOne(r => r.RegistroDeActividad)
                .WithOne(af => af.Rutina)
                .HasForeignKey<Rutina>(r => new { Id = r.IdActividad, r.IdPerfil });
            // Relación uno a muchos entre Usuario y LlaveDeAPI
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.LlavesDeAPI)
                .WithOne(ll => ll.Usuario)
                .HasForeignKey(ll => ll.IdUsuario);
        }
    }
}