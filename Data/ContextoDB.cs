using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;

namespace ServicioHydrate.Data 
{
    public class ContextoDB : DbContext 
    {
        public ContextoDB(DbContextOptions<ContextoDB> opciones)
            : base(opciones)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<RecursoInformativo> Recursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {

        }
    }
}