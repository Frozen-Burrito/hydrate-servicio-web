using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Data;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Route("api/v1/recursos")]
    // [Authorize(Roles = "ADMIN_RECURSOS_INF")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorRecursosInformativos : ControllerBase
    {
        /// El repositorio de acceso a los Recursos Informativos.
        private readonly IServicioRecursos _repoRecursos;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorRecursosInformativos> _logger;

        private const string EsquemaJwt = JwtBearerDefaults.AuthenticationScheme;
        private const string EsquemaApiKey = OpcionesAuthLlaveDeAPI.AuthenticationString;

        private const string EsquemaJwtAndApiKey = $"{EsquemaJwt},{EsquemaApiKey}";

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
        [HttpGet]
        [Authorize(AuthenticationSchemes = EsquemaJwtAndApiKey)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORecursoInformativo>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecursosInformativos([FromQuery] DTOParamsPagina? paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var recursosInf = await _repoRecursos.GetRecursosAsync(paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTORecursoInformativo>
                                .DesdeColeccion(recursosInf, numPagina, ruta);

                return Ok(resultado);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Busca y retorna el Recurso Informativo con el idRecurso deseado.
        [HttpGet("{idRecurso}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORecursoInformativo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecursoPorId(int idRecurso)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";
            
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

        /// <summary>
        /// Agrega un nuevo Recurso Informativo a la colección.
        /// </summary>
        /// <param name="nuevoRecurso">El recurso para agregar.</param>
        /// <returns>Resultado de accion HTTP</returns>
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
            string ruta = Request.Path.Value ?? "ruta no identificada";
            
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var recursoAgregado = await _repoRecursos.AgregarNuevoRecursoAsync(nuevoRecurso);

                return CreatedAtAction(
                    nameof(GetRecursoPorId),
                    new { idRecurso = recursoAgregado.Id },
                    recursoAgregado
                );
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

        /// <summary>
        /// Actualiza el registro del Recurso Informativo con [idRecurso].
        /// </summary>
        /// <param name="idRecurso">El identificador del recurso.</param>
        /// <param name="recursoModificado">Las modificaciones realizadas al recurso.</param>
        /// <returns>Resultado de accion HTTP</returns>
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
            string ruta = Request.Path.Value ?? "ruta no identificada";
            
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

        /// <summary>
        /// Elimina el Recurso Informativo con [idRecurso].
        /// </summary>
        /// <param name="idRecurso">El identificador del recurso.</param>
        /// <returns>Resultado de accion HTTP</returns>
        [HttpDelete("{idRecurso}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarRecurso(int idRecurso)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";
            
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
#nullable disable