using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;

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
        }
    }
}