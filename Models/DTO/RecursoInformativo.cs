using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos
{
    public class RecursoInformativo
    {
        public int Id { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }

        public DateTime FechaPublicacion { get; set; }
    }
}