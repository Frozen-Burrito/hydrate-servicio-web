using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable
namespace ServicioHydrate.Modelos.DTO 
{
    public class DTORangoFechas 
    {
        private DateTime? _desde;

        [NotMapped]
        public DateTime? desdeDate { get => _desde; }

        public string? Desde
        {
            get => _desde?.ToString("o"); 
            set 
            { 
                DateTime fechaParseada;
                bool esFechaValida = DateTime.TryParse(value, out fechaParseada);

                if (esFechaValida) 
                {
                    _desde = fechaParseada;
                }
            }
        }

        private DateTime? _hasta;

        [NotMapped]
        public DateTime? hastaDate { get => _hasta; }

		public string? Hasta 
        { 
            get => _hasta?.ToString("o"); 
            set 
            { 
                DateTime fechaParseada;
                bool esFechaValida = DateTime.TryParse(value, out fechaParseada);

                if (esFechaValida) 
                {
                    _hasta = fechaParseada;
                }
            }
        }
    }
}
#nullable disable