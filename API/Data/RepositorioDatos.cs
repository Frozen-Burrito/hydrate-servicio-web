using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.Datos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioDatos : IServicioDatos
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioDatos(ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public async Task AgregarActividadFisica(int idPerfil, ICollection<DTORegistroActividad> nuevasActividades)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            var mapaTiposDeAct = new Dictionary<int, DatosDeActividad>();

            List<DatosDeActividad> datosDeActividades = await _contexto.DatosDeActividades
                .ToListAsync();

            datosDeActividades.ForEach((datosDeAct) => mapaTiposDeAct.Add(datosDeAct.Id, datosDeAct));

            IEnumerable<ActividadFisica> registros = nuevasActividades
                .Select(ra => ra.ComoNuevoModelo(
                    datosDeActividades[ra.IdTipoDeActividad],
                    perfil, 
                    null,
                    esParteDeDatosAbiertos: false
                ));

            _contexto.AddRange(registros);
            await _contexto.SaveChangesAsync();
        }

        public async Task AgregarHidratacion(int idPerfil, ICollection<DTORegistroDeHidratacion> nuevosRegistrosHidr)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IEnumerable<RegistroDeHidratacion> registros = nuevosRegistrosHidr
                .Select(rh => rh.ComoNuevoModelo(
                    perfil, 
                    esParteDeDatosAbiertos: false
                ));

            foreach (var registroHidratacion in registros) 
            {
                _contexto.Entry(registroHidratacion).State = EntityState.Modified;
            }

            await _contexto.SaveChangesAsync();
        }

        public async Task<String?> NotificarAlertaBateria(Guid idCuentaUsuario, int idPerfil)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .Include(p => p.Configuracion)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            String? fcmMessageId = null;

            // Intentar enviar una notificación de alerta de batería, si están activadas
            // y los registros de hidratación más recientes incluyen un nivel de batería bajo.
            if (perfil.Configuracion.NotificacionesPermitidas.Contains(TiposDeNotificacion.ALERTAS_BATERIA_DISPOSITIVO))
            {                
                IQueryable<RegistroDeHidratacion>? registrosRecientes = _contexto.RegistrosDeHidratacion
                    .Include(rh => rh.PerfilDeUsuario)
                    .AsSplitQuery()
                    .Where(rh => (rh.PerfilDeUsuario.Id.Equals(idPerfil)))
                    .OrderBy(rh => rh.Fecha)
                    .Take(1);

                if (registrosRecientes is not null && registrosRecientes.Count() > 0) 
                {
                    int nivelDeBateriaBaja = registrosRecientes.Last().PorcentajeCargaBateria;
                    DateTime fechaMasRecienteConBateriaBaja = registrosRecientes.Last().Fecha;
                    TimeSpan diferenciaDeFechas = DateTime.Now.Subtract(fechaMasRecienteConBateriaBaja);
                    if (diferenciaDeFechas.TotalHours < 4) 
                    {
                        // Existe un registro de hidratación reciente con batería
                        // baja. Notificar al usuario.
                        var tokenDeRegistroFCM = await _contexto.TokensParaNotificaciones
                            .Include(t => t.Perfil)
                            .AsSplitQuery()
                            .Where(t => (t.Perfil.IdCuentaUsuario.Equals(idCuentaUsuario) && t.Perfil.Id.Equals(idPerfil)))
                            .FirstOrDefaultAsync();

                        var instanciaFCM = FirebaseMessaging.DefaultInstance;

                        if (tokenDeRegistroFCM is not null && instanciaFCM is not null) 
                        {
                            var mensaje = new Message()
                            {
                                Token = tokenDeRegistroFCM.Token,
                                Notification = new Notification()
                                {
                                    Title = "La batería de tu extensión Hydrate está baja",
                                    Body = $"Tu extensión tiene {nivelDeBateriaBaja}% de carga restante. Conéctala para recargarla.",
                                    ImageUrl = "https://servicio-web-hydrate.azurewebsites.net/favicon.ico",
                                },
                            };

                            // Enviar el mensaje a través de FCM.
                            fcmMessageId = await instanciaFCM.SendAsync(mensaje);
                        }
                    }
                }
            } 

            return fcmMessageId;
        }

        public async Task AgregarMetas(int idPerfil, ICollection<DTOMeta> nuevasMetas)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            var mapaDeEtiquetas = new Dictionary<int, Etiqueta>();

            List<Etiqueta> etiquetasExistentes = await _contexto.Etiquetas
                .Where(e => e.IdPerfil.Equals(idPerfil))
                .ToListAsync();

            etiquetasExistentes.ForEach((etiqueta) => mapaDeEtiquetas.Add(etiqueta.Id, etiqueta));

            IEnumerable<Meta> metasAgregadas = nuevasMetas
                .Select(m => m.ComoNuevoModelo(
                    etiquetasExistentes,
                    perfil
                ));

            _contexto.AddRange(metasAgregadas);
            await _contexto.SaveChangesAsync();
        }

        public async Task AgregarRutinas(int idPerfil, ICollection<DTORutina> nuevasRutinas)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IEnumerable<Rutina> rutinas = nuevasRutinas
                .Select(ru => ru.ComoNuevoModelo(perfil));

            _contexto.AddRange(rutinas);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarMeta(int idPerfil, int idMeta)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0 || await _contexto.Metas.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario o la meta.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            Meta? meta = await _contexto.Metas
                .Where(m => m.Id.Equals(idMeta) && m.IdPerfil.Equals(idPerfil))
                .FirstOrDefaultAsync();

            if (meta is null)
            {
                throw new ArgumentException("No existe una meta con el ID especificado.");
            }

            _contexto.Metas.Remove(meta);
            await _contexto.SaveChangesAsync();
        }

        public async Task<ICollection<DTORegistroActividad>> GetActividadesPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina)
        {
            int numActividades = await _contexto.RegistrosDeActFisica.CountAsync();

            if (numActividades <= 0)
            {
                // Si no existe ningún registro de actividad física, evitar hacer más queries.
                return new List<DTORegistroActividad>();
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IQueryable<DTORegistroActividad>? actividades = _contexto.RegistrosDeActFisica
                .Where(ra => ra.IdPerfil == perfil.Id)
                .Include(ra => ra.DatosActividad)
                .Include(ra => ra.Rutina)
                .AsSplitQuery()
                .OrderBy(ra => ra.Fecha)
                .Select(ra => ra.ComoDTO());

            ListaPaginada<DTORegistroActividad> registrosActFisicaPaginados = await ListaPaginada<DTORegistroActividad>
                .CrearAsync(actividades, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return registrosActFisicaPaginados;
        }

        public async Task<ICollection<DTORegistroDeHidratacion>> GetHidratacionPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina)
        {
            int numRegistrosHidratacion = await _contexto.RegistrosDeHidratacion.CountAsync();

            if (numRegistrosHidratacion <= 0)
            {
                // Si no existe ningún registro de hidratación, evitar hacer más queries.
                return new List<DTORegistroDeHidratacion>();
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IQueryable<DTORegistroDeHidratacion>? regHidratacion = _contexto.RegistrosDeHidratacion
                .Include(rh => rh.PerfilDeUsuario)
                .AsSplitQuery()
                .Where(rh => rh.PerfilDeUsuario.Id == perfil.Id)
                .OrderBy(rh => rh.Fecha)
                .Select(rh => rh.ComoDTO());

            ListaPaginada<DTORegistroDeHidratacion> registrosHidratacionPaginados = await ListaPaginada<DTORegistroDeHidratacion>
                .CrearAsync(regHidratacion, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return registrosHidratacionPaginados;
        }

        public async Task<ICollection<DTOMeta>> GetMetasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina)
        {
            int numMetasExistentes = await _contexto.Metas.CountAsync();

            if (numMetasExistentes <= 0)
            {
                // Si no existe ningún registro de hidratación, evitar hacer más queries.
                return new List<DTOMeta>();
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IQueryable<DTOMeta>? metasDelPerfil = _contexto.Metas
                .Where(m => m.IdPerfil == perfil.Id)
                .Include(m => m.Etiquetas)
                .AsSplitQuery()
                .OrderBy(m => m.FechaInicio)
                .Select(m => m.ComoDTO());

            ListaPaginada<DTOMeta> metasPaginadas = await ListaPaginada<DTOMeta>
                .CrearAsync(metasDelPerfil, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return metasPaginadas;
        }

        public async Task<ICollection<DTORutina>> GetRutinasPorPerfil(int idPerfil, DTOParamsPagina? paramsPagina)
        {
            int numRutinasExistentes = await _contexto.RutinasDeActFisica.CountAsync();

            if (numRutinasExistentes <= 0)
            {
                // Si no existe ningún registro de hidratación, evitar hacer más queries.
                return new List<DTORutina>();
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            IQueryable<DTORutina>? rutinas = _contexto.RutinasDeActFisica
                .Where(ru => ru.IdPerfil == perfil.Id)
                .OrderBy(ru => ru.Id)
                .Select(ru => ru.ComoDTO());

            ListaPaginada<DTORutina> registrosHidratacionPaginados = await ListaPaginada<DTORutina>
                .CrearAsync(rutinas, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return registrosHidratacionPaginados;
        }
    }
}
#nullable disable