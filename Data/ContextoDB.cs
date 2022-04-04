using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;

namespace ServicioHydrate.Data 
{
    /// Permite la interacción con la base de datos en MySQL y las 
    /// entidades de EF core. 
    /// Se utilizan dos contextos de DB diferentes para administrar las 
    /// migraciones de los proveedores especificos.  
    public class ContextoDB : DbContext
    {
        public ContextoDB(DbContextOptions<ContextoDB> opciones)
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

        /// Configura la creación de cada entidad en la base de datos. (No la inserción)
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()");

            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);
        }
    }
}