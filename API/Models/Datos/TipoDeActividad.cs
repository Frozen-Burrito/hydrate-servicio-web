using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO.Datos; 

namespace ServicioHydrate.Modelos.Datos
{
    [Table("TiposDeActividad")]
    public class TipoDeActividad 
    {   
        [Key]
        public int Id { get; set; } 

        [Range(0.0, 25.0)]
        public double METs { get; set; }

        [Range(0.0, 50.0)]
        public double VelocidadPromedioKMH { get; set; }

        public int IdActividadGoogleFit { get; set; }

        public virtual ICollection<RegistroDeActividad> RegistrosDeActividad { get; set; }

        public DTODatosActividad ComoDTO()
        {
            return new DTODatosActividad
            {
                Id = this.Id,
                METs = this.METs,
                VelocidadPromedioKMH = this.VelocidadPromedioKMH,
            };
        }
    }
}
