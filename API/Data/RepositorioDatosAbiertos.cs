using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.Datos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Data
{
    public class RepositorioDatosAbiertos : IServicioDatosAbiertos
    {
        private readonly ContextoDBMysql _contexto;

        public RepositorioDatosAbiertos(ContextoDBMysql contexto)
        {
            this._contexto = contexto;
        }

        public async Task AportarDatosDeActividad(Perfil perfil, IEnumerable<DTONuevaActividad> datos)
        {
            var mapaTiposDeAct = new Dictionary<int, TipoDeActividad>();

            List<TipoDeActividad> datosDeActividades = await _contexto.DatosDeActividades
                .ToListAsync();

            datosDeActividades.ForEach((datosDeAct) => mapaTiposDeAct.Add(datosDeAct.Id, datosDeAct));

            IEnumerable<RegistroDeActividad> registros = datos
                .Select(ra => ra.ComoNuevoModelo(
                    datosDeActividades[ra.IdTipoDeActividad],
                    null, 
                    esParteDeDatosAbiertos: true
                ));

            _contexto.AddRange(registros);
            await _contexto.SaveChangesAsync();
        }

        public async Task AportarDatosDeHidratacion(Perfil perfil, IEnumerable<DTORegistroDeHidratacion> datos)
        {
            IEnumerable<RegistroDeHidratacion> registros = datos
                .Select(rh => rh.ComoNuevoModelo(perfil, esParteDeDatosAbiertos: true));

            _contexto.AddRange(registros);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> ExportarDatosAbiertos(TipoDeDatosAbiertos tipoDeDatos, FiltrosPorPerfil filtros)
        {
            switch (tipoDeDatos) 
            {
                case TipoDeDatosAbiertos.HIDRATACION:
                    if (await _contexto.RegistrosDeHidratacion.CountAsync() <= 0)
                    {
                        return new List<DTORegistroDeHidratacion>();
                    }

                    IEnumerable<DTORegistroDeHidratacion> registrosHidr = await _contexto.RegistrosDeHidratacion
                        .OrderByDescending(rh => rh)
                        .Select(rh => rh.ComoDTO())
                        .ToListAsync();

                    return registrosHidr;

                case TipoDeDatosAbiertos.ACTIVIDAD_FISICA:
                    if (await _contexto.RegistrosDeActFisica.CountAsync() <= 0)
                    {
                        return new List<DTORegistroActividad>();
                    }

                    IEnumerable<DTORegistroActividad> registrosAct = await _contexto.RegistrosDeActFisica
                        .OrderByDescending(ra => ra)
                        .Select(ra => ra.ComoDTO())
                        .ToListAsync();

                    return registrosAct;

                default:
                    return new List<object>();
            } 
        }

        public async Task<ICollection<DTORegistroActividad>> GetDatosDeActividad(FiltrosPorPerfil filtros, DTOParamsPagina paramsPagina)
        {
            if (await _contexto.RegistrosDeActFisica.CountAsync() == 0) 
            {
                return (ICollection<DTORegistroActividad>) new List<DTORegistroActividad>();
            }

            ICollection<DTORegistroActividad> registrosActividad = await _contexto.RegistrosDeActFisica
                .Where(ra => ra.EsInformacionAbierta)
                .OrderByDescending(ra => ra.Id)
                .Select(ra => ra.ComoDTO())
                .ToListAsync();

            return registrosActividad;
        }

        public async Task<ICollection<DTORegistroDeHidratacion>> GetDatosDeHidratacion(FiltrosPorPerfil filtros, DTOParamsPagina paramsPagina)
        {
            if (await _contexto.RegistrosDeHidratacion.CountAsync() == 0) 
            {
                return (ICollection<DTORegistroDeHidratacion>) new List<DTORegistroDeHidratacion>();
            }

            ICollection<DTORegistroDeHidratacion> datos = await _contexto.RegistrosDeHidratacion
                .Where(rh => rh.EsInformacionAbierta)
                .OrderByDescending(rh => rh.Id)
                .Select(rh => rh.ComoDTO())
                .ToListAsync();

            return datos;
        }
    }
}