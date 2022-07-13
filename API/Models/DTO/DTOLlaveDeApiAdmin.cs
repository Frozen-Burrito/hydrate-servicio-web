using System;


namespace ServicioHydrate.Modelos.DTO
{
    public class DTOLlaveDeAPIAdmin
    {
        public int Id { get; set; }

        public Guid IdPropietario { get; set; }

        public String NombrePropietario { get; set; }

        public String EmailPropietario { get; set; }

        public string NombreDelCliente { get; set; }

        public int NumeroDePeticiones { get; set; }

        public bool TuvoActividadEnMesPasado { get; set; }
    }
}