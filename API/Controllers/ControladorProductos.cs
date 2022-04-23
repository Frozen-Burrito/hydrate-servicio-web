using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Authorize(Roles = "ADMIN_ORDENES")]
    [Route("api/v1/productos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorProductos : ControllerBase
    {
        /// El repositorio de acceso a las Ordenes.
        private readonly IServicioProductos _repositorioProductos;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorProductos> _logger;

        public ControladorProductos(
            IServicioProductos servicioProductos,
            ILogger<ControladorProductos> logger
        )
        {
            // Asegurar que el controlador recibe una instancia del repositorio.
            if (servicioProductos is null) 
            {
                throw new ArgumentNullException(nameof(servicioProductos));
            }

            this._repositorioProductos = servicioProductos;
            this._logger = logger;
        }

        /// <summary>
        /// Retorna todos los productos.
        /// </summary>
        /// <param name="soloDisponibles">Especifica si se deben obtener solo los productos disponibles.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DTOProducto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductos(bool soloDisponibles = false)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener todos los productos, segun los filtros recibidos.
                var productos = await _repositorioProductos.GetProductos(soloDisponibles);

                return Ok(productos);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Busca un producto con el idProducto recibido.
        /// </summary>
        /// <param name="idProducto">El Id del producto.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("{idProducto}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOProducto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductoPorId(int idProducto)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var productos = await _repositorioProductos.GetProductoPorId(idProducto);

                return Ok(productos);
            }
            catch (ArgumentException e)
            {
                // No existe un producto con el ID solicitado. Retorna 404.
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
        /// Registra un nuevo producto en el servicio.
        /// </summary>
        /// <param name="nuevaOrden">El producto para registrar.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> NuevoProducto(DTONuevoProducto nuevoProducto)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var productoCreado = await _repositorioProductos.NuevoProducto(nuevoProducto);

                return CreatedAtAction(
                    nameof(GetProductoPorId),
                    new { idProducto = productoCreado.Id },
                    productoCreado
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
        /// Actualiza la cantidad de unidades disponibles de un producto.
        /// </summary>
        /// <param name="idProducto">El Id del producto.</param>
        /// <param name="nuevaCantidad">La cantidad actualizada de unidades.</param>
        /// <returns></returns>
        [HttpPatch("{idProducto}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOProducto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ModificarUnidadesDisponibles(int idProducto, int nuevaCantidad)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var productoModificado = await _repositorioProductos
                    .ModificarCantidadDisponible(idProducto, nuevaCantidad);

                return Ok(productoModificado);
            }
            catch (ArgumentException e)
            {
                // No existe un producto con el ID solicitado. Retorna 404.
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
        /// Elimina un producto.
        /// </summary>
        /// <param name="idProducto">El Id del producto.</param>
        /// <returns></returns>
        [HttpDelete("{idProducto}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> EliminarProducto(int idProducto)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                await _repositorioProductos.EliminarProducto(idProducto);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un producto con el ID solicitado. Retorna 404.
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