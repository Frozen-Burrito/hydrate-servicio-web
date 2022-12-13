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
        Task<ICollection<DTOMeta>> GetMetasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina, DTORangoFechas? rangoFechas);

        Task<ICollection<DTORegistroDeHidratacion>> GetHidratacionPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina, DTORangoFechas? rangoFechas);

        Task<ICollection<DTORutina>> GetRutinasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina, DTORangoFechas? rangoFechas);

        Task<ICollection<DTORegistroActividad>> GetActividadesPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina, DTORangoFechas? rangoFechas);

        Task AgregarMetas(int idPerfil, ICollection<DTOMeta> metas);

        Task AgregarHidratacion(int idPerfil, ICollection<DTORegistroDeHidratacion> registrosDeHidratacion);

        Task AgregarRutinas(int idPerfil, ICollection<DTORutina> rutinas);

        Task AgregarActividadFisica(int idPerfil, ICollection<DTORegistroActividad> registrosDeActividad);

        Task AgregarEtiquetas(int idPerfil, ICollection<DTOEtiqueta> etiquetasAgregadas);

        Task EliminarMeta(int idPerfil, int idMeta);

        Task<String?> NotificarAlertaBateria(Guid idCuentaUsuario, int idPerfil);

        Task<String?> NotificarRecordatorioDescanso(Guid idCuentaUsuario, int idPerfil);
    }
}
#nullable disable