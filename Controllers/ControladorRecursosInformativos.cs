using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using ServicioHydrate.Data;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace ServicioHydrate.Controllers
{
    [Autorizacion]
    [ApiController]
    [Route("api/v1/recursos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorRecursosInformativos : ControllerBase
    {
        /// El repositorio de acceso a los Recursos Informativos.
        private readonly IServicioRecursos _repoRecursos;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorRecursosInformativos> _logger;

        public ControladorRecursosInformativos(
            IServicioRecursos servicioRecursos,
            ILogger<ControladorRecursosInformativos> logger
        )
        {
            // Asegurar que el controlador tiene una instancia del repositorio.
            if (servicioRecursos is null) 
            {
                throw new ArgumentNullException(nameof(servicioRecursos));
            }

            this._repoRecursos = servicioRecursos;
            this._logger = logger;
        }

        /// Regresa todos los Recursos Informativos disponibles.
        [PermitirAnonimo]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DTORecursoInformativo>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecursosInformativos()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var recursosInf = await _repoRecursos.GetRecursosAsync();

                return Ok(recursosInf);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Busca y retorna el Recurso Informativo con el idRecurso deseado.
        [PermitirAnonimo]
        [HttpGet("{idRecurso}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORecursoInformativo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecursoPorId(int idRecurso)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var recurso = await _repoRecursos.GetRecursoPorIdAsync(idRecurso);

                return Ok(recurso);
            }
            catch (ArgumentException e)
            {
                // No existe un recurso informativo con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Agrega un nuevo Recurso Informativo a la colección de recursos.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarRecurso(DTORecursoInformativo nuevoRecurso)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}: {nuevoRecurso.Id}, {nuevoRecurso.Titulo}");

            try 
            {
                var recursoAgregado = await _repoRecursos.AgregarNuevoRecursoAsync(nuevoRecurso);

                return CreatedAtAction(
                    nameof(GetRecursoPorId),
                    new { idRecurso = recursoAgregado.Id },
                    recursoAgregado
                );
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Actualiza el registro del Recurso Informativo con idRecurso.
        [HttpPut("{idRecurso}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarRecurso(int idRecurso, DTORecursoInformativo recursoModificado)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                recursoModificado.Id = idRecurso;
                await _repoRecursos.ActualizarRecursoAsync(recursoModificado);

                return NoContent();
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Elimina un RecursoInformativo con el idRecurso especificado.
        [HttpDelete("{idRecurso}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarRecurso(int idRecurso)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                await _repoRecursos.EliminarRecursoAsync(idRecurso);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un recurso informativo con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }
    }
}