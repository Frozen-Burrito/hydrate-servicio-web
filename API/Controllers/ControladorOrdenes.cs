using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Authorize(Roles = "ADMIN_ORDENES")]
    [Route("api/v1/ordenes")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorOrdenes : ControllerBase
    {
        /// El repositorio de acceso a las Ordenes.
        private readonly IServicioOrdenes _repositorioOrdenes;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorOrdenes> _logger;

        public ControladorOrdenes(
            IServicioOrdenes servicioComentarios,
            ILogger<ControladorOrdenes> logger
        )
        {
            // Asegurar que el controlador recibe una instancia del repositorio.
            if (servicioComentarios is null) 
            {
                throw new ArgumentNullException(nameof(servicioComentarios));
            }

            this._repositorioOrdenes = servicioComentarios;
            this._logger = logger;
        }

        /// <summary>
        /// Retorna todas las ordenes creadas, segun filtros de fecha y estado.
        /// </summary>
        /// <param name="desde">Especifica el inicio del rango de fechas.</param>
        /// <param name="hasta">Especifica el final del rango de fechas</param>
        /// <param name="estado">El estado de las ordenes retornadas.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet]
        [Authorize(Roles = "ADMIN_ORDENES,NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenesConFiltros(DateTime? desde, DateTime? hasta, EstadoOrden? estado)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {       
                // Obtener el ID del usuario actual desde el JWT.
                Guid idUsuarioActual = new Guid(this.User.Claims.First(i => i.Type == "id").Value);

                // Obtener todas las ordenes, segun los filtros recibidos.
                var ordenes = await _repositorioOrdenes.GetOrdenes(
                    idUsuarioActual,
                    fechaDesde: desde,
                    fechaHasta: hasta, 
                    estado: estado
                );

                return Ok(ordenes);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("{idUsuario}")]
        [Authorize(Roles = "ADMIN_ORDENES,NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenesDeUsuario(Guid idUsuario, EstadoOrden? estado)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el rol y el ID del usuario actual desde el JWT.
                Guid idUsuarioActual = new Guid(this.User.Claims.First(i => i.Type == "id").Value);
                string rol = this.User.Claims.First(i => i.Type == ClaimTypes.Role).Value;

                if (RolDeUsuario.ADMIN_ORDENES.ToString() != rol && idUsuario != idUsuarioActual) 
                { 
                    // Si el usuario actual no es un administrador de órdenes y solicita
                    // obtener órdenes de un usuario que no es él, retornar un No Autorizado.
                    return Unauthorized();
                }

                var ordenes = await _repositorioOrdenes.GetOrdenesDeUsuario(idUsuario, estado);

                return Ok(ordenes);
            }
            catch (ArgumentException e)
            {
                // No existe un usuario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("{idOrden}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenPorId(Guid idOrden)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var orden = await _repositorioOrdenes.GetOrdenPorId(idOrden);

                return Ok(orden);
            }
            catch (ArgumentException e)
            {
                // No existe una orden con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("buscar")]
        [Authorize(Roles = "ADMIN_ORDENES,NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarOrdenes(string nombre, string email, Guid idOrden, EstadoOrden? estado)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var ordenes = await _repositorioOrdenes.BuscarOrdenes(nombre, email, idOrden, estado);

                return Ok(ordenes);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Registra una nueva orden, con un estado por defecto de "Pendiente".
        /// </summary>
        /// <param name="nuevaOrden">Los productos y cantidades de la nueva orden.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CrearOrden(DTONuevaOrden nuevaOrden)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener el ID del usuario actual desde el JWT.
                Guid idUsuarioActual = new Guid(this.User.Claims.First(i => i.Type == "id").Value);

                var ordenCreada = await _repositorioOrdenes.NuevaOrden(nuevaOrden, idUsuarioActual);

                return CreatedAtAction(
                    nameof(GetOrdenPorId),
                    new { idComentario = ordenCreada.Id },
                    ordenCreada
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

        /// <summary>
        /// Modifica el estado de la orden con id = idOrden.
        /// </summary>
        /// <remarks>
        /// El estado de una orden solo puede ser actualizado en cierto orden:
        /// PENDIENTE -> EN_PROGRESO -> CONCLUIDA O ERROR.
        /// </remarks>
        /// <param name="idOrden">La orden que se va a actualizar.</param>
        /// <param name="nuevoEstado">El nuevo estado para la orden.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idOrden}/actualizar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarEstadoDeOrden(Guid idOrden, EstadoOrden nuevoEstado)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {                
                await _repositorioOrdenes.ModificarEstadoDeOrden(idOrden, nuevoEstado);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (InvalidOperationException)
            {
                // Se solicito un cambio de estado no soportado. Retorna 405.
                return Problem(
                    "No es posible actualizar el estado de la orden con el nuevo estado recibido.",
                    statusCode: StatusCodes.Status405MethodNotAllowed
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
    }
}