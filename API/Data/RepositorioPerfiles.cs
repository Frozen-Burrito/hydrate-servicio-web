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

        public async Task<DTOPerfil> GetPerfilPorId(Guid idCuentaUsuario, int idPerfil)
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

            return perfil.ComoDTO();
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