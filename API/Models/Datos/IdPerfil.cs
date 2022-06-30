using System;

namespace ServicioHydrate.Modelos.Keys
{
    public class IdPerfil 
    {
        public Guid IdUsuario { get; set; }

        public int Id { get; set; }

        public IdPerfil(Guid IdUsuario, int Id)
        {
            this.IdUsuario = IdUsuario;
            this.Id = Id;
        }
    }
}