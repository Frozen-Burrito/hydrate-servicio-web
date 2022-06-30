using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.Datos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Data
{
    public class RepositorioDatosAbiertos : IServicioDatosAbiertos
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioDatosAbiertos(ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public Task AportarDatosDeActividad(IEnumerable<DTOActividad> datos)
        {
            throw new System.NotImplementedException();
        }

        public Task AportarDatosDeHidratacion(IEnumerable<DTORegistroDeHidratacion> datos)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<object>> ExportarDatosAbiertos(TipoDeDatosAbiertos tipoDeDatos, FiltrosPorPerfil filtros)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<DTOActividad>> GetDatosDeActividad(FiltrosPorPerfil filtros, DTOParamsPagina paramsPagina)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<DTORegistroDeHidratacion>> GetDatosDeHidratacion(FiltrosPorPerfil filtros, DTOParamsPagina paramsPagina)
        {
            throw new System.NotImplementedException();
        }
    }
}