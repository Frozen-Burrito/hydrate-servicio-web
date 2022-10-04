using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioPerfiles : IServicioPerfil
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioPerfiles(IWebHostEnvironment env, ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public async Task ActualizarPerfil(DTOPerfil modificacionesPerfil)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            int idPerfil = modificacionesPerfil.Id;
            Guid idUsuario = modificacionesPerfil.IdCuentaUsuario;

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id.Equals(idPerfil) && p.IdCuentaUsuario.Equals(idUsuario))
                .Include(p => p.EntornosDesbloqueados)
                .Include(o => o.PaisDeResidencia)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (perfil is null) 
            {
                throw new ArgumentException("No existe un perfil con el ID especificado");
            }

            if (perfil.NumModificaciones > 3)
            {
                throw new InvalidOperationException("El perfil ya llegó al límite de modificaciones.");
            }
            
            Pais? paisActualizado = await _contexto.Paises
                .Where(p => p.Id == modificacionesPerfil.PaisDeResidencia.Id)
                .FirstOrDefaultAsync();

            if (paisActualizado is null) 
            {
                throw new ArgumentException("No existe un pais con el ID especificado");
            }

            ICollection<Entorno> entornosActualizados = await _contexto.Entornos
                .Where(e => modificacionesPerfil.IdsEntornosDesbloqueados.Contains(e.Id))
                .ToListAsync();

            perfil.Actualizar(modificacionesPerfil, paisActualizado, entornosActualizados);

            _contexto.Entry(perfil).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarTokenFCM(Guid idCuentaUsuario, int idPerfil, DTOTokenFCM tokenActualizado)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.IdCuentaUsuario.Equals(idCuentaUsuario) && p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            TokenFCM? tokenFCM = null;

            if (await _contexto.Perfiles.CountAsync() > 0) 
            {
                tokenFCM = await _contexto.TokensParaNotificaciones
                    .Include(t => t.Perfil)
                    .AsSplitQuery()
                    .Where(t => (t.Perfil.IdCuentaUsuario.Equals(idCuentaUsuario) && t.Perfil.Id.Equals(idPerfil)))
                    .FirstOrDefaultAsync();
            }

            if (tokenFCM is null) 
            {
                 _contexto.TokensParaNotificaciones.Add(tokenActualizado.ComoNuevoModelo());
            } else 
            {
                bool debeBorrarTokenExistente = String.IsNullOrEmpty(tokenActualizado.Token.Trim());

                if (debeBorrarTokenExistente) 
                {
                    _contexto.TokensParaNotificaciones.Remove(tokenFCM);
                } else 
                {
                    tokenFCM.Actualizar(tokenActualizado);
                    _contexto.Entry(tokenFCM).State = EntityState.Modified;
                }
            }

            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarPerfil(Guid idCuentaUsuario, int idPerfil)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.IdCuentaUsuario.Equals(idCuentaUsuario) && p.Id == idPerfil)
                .FirstOrDefaultAsync();

            if (perfil is null)
            {
                throw new ArgumentException("No existe un perfil con el ID especificado.");
            }

            _contexto.Perfiles.Remove(perfil);
            await _contexto.SaveChangesAsync();
        }

        public async Task<DTOConfiguracion> GetConfiguracionDelPerfil(Guid idCuentaUsuario, int idPerfil)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id.Equals(idPerfil) && p.IdCuentaUsuario.Equals(idCuentaUsuario))
                .FirstOrDefaultAsync();

            if (perfil is null) 
            {
                throw new ArgumentException("No existe un perfil con el ID especificado");
            }

            Configuracion? config = await _contexto.Configuraciones
                .Include(c => c.Perfil)
                .AsSplitQuery()
                .Where(c => (c.Perfil.Id == perfil.Id && c.Perfil.IdCuentaUsuario == idCuentaUsuario))
                .FirstOrDefaultAsync();

            // Si no hay un registro con la configuración del usuario, crear 
            // uno nuevo con configuración por defecto.
            if (config is null) 
            {
                config = Configuracion.PorDefecto();
                _contexto.Configuraciones.Add(config);
                await _contexto.SaveChangesAsync();

                if (config is null)
                {
                    throw new Exception("No fue posible obtener la configuración del perfil");
                }
            }

            return config.ComoDTO();
        }

        public async Task<Perfil> GetPerfilPorId(Guid idCuentaUsuario, int idPerfil)
        {
            if (await _contexto.Perfiles.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe el perfil de usuario solicitado.");
            }

            Perfil? perfil = await _contexto.Perfiles
                .Where(p => p.Id.Equals(idPerfil) && p.IdCuentaUsuario.Equals(idCuentaUsuario))
                .Include(p => p.EntornosDesbloqueados)
                .Include(o => o.PaisDeResidencia)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (perfil is null) 
            {
                throw new ArgumentException("No existe un perfil con el ID especificado");
            }

            return perfil;
        }

        public async Task<DTOConfiguracion> ModificarConfiguracionDelPerfil(Guid idCuentaUsuario, int idPerfil, DTOConfiguracion cambiosDeConfiguracion)
        {
            if (await _contexto.Configuraciones.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existen configuraciones registradas para perfiles de usuario.");
            }

            Configuracion? config = await _contexto.Configuraciones
                .Include(c => c.Perfil)
                .AsSplitQuery()
                .Where(c => c.Perfil.Id.Equals(idPerfil) && c.Perfil.IdCuentaUsuario.Equals(idCuentaUsuario))
                .FirstOrDefaultAsync();

            if (config is null) 
            {
                config = Configuracion.PorDefecto();
            }

            config.Actualizar(cambiosDeConfiguracion);

            if (!config.TieneNotificacionesActivadas) 
            {
                var perfil = config.Perfil;
                // Si el usuario desactivó las notificaciones, eliminar su
                // token de registro de FCM para evitar tener tokens viejos.
                var tokenFCM = await _contexto.TokensParaNotificaciones
                    .Include(t => t.Perfil)
                    .AsSplitQuery()
                    .Where(t => (t.Perfil.IdCuentaUsuario.Equals(perfil.IdCuentaUsuario) && t.Perfil.Id.Equals(perfil.Id)))
                    .FirstOrDefaultAsync();

                if (tokenFCM is not null) 
                {
                    _contexto.TokensParaNotificaciones.Remove(tokenFCM);
                }
            }

            _contexto.Entry(config).State = EntityState.Modified;
            int numOfEntriesWritten = await _contexto.SaveChangesAsync();

            // Revisar si no fue posible persistir los cambios.
            if (numOfEntriesWritten <= 0) 
            {
                throw new DbUpdateException("No fue posible persistir los cambios de configuración.");
            }

            return config.ComoDTO();
        }

        public async Task<DTOPerfil> RegistrarPerfilExistente(DTOPerfil perfil, DTOUsuario cuentaUsuario)
        {
            Pais? pais = await _contexto.Paises.FindAsync(perfil.PaisDeResidencia.Id);

            if (pais is null)
            {
                throw new ArgumentException("No existe el pais con el ID especificado.");
            }

            Perfil modeloPerfil = perfil.ComoNuevoModelo(cuentaUsuario.Id, pais);

            _contexto.Perfiles.Add(modeloPerfil);
            await _contexto.SaveChangesAsync();

            return modeloPerfil.ComoDTO();
        }
    }
}