using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;

#nullable enable
namespace ServicioHydrate.Data
{
    public interface IServicioDatos 
    {
        Task<ICollection<DTOMeta>> GetMetasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina);

        Task<ICollection<DTORegistroDeHidratacion>> GetHidratacionPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina);

        Task<ICollection<DTORutina>> GetRutinasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina);

        Task<ICollection<DTORegistroActividad>> GetActividadesPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina);

        Task AgregarMetas(int idPerfil, ICollection<DTOMeta> nuevasMetas);

        Task AgregarHidratacion(int idPerfil, ICollection<DTORegistroDeHidratacion> nuevosRegistrosHidr);

        Task AgregarRutinas(int idPerfil, ICollection<DTORutina> nuevasRutinas);

        Task AgregarActividadFisica(int idPerfil, ICollection<DTORegistroActividad> nuevasActividades);

        Task EliminarMeta(int idPerfil, int idMeta);
    }
}
#nullable disable