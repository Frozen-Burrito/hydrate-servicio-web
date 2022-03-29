using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;

namespace ServicioHydrate.Data 
{
    /// Permite la interacción con la base de datos y las entidades de EF core. 
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