using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Stripe;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    // [Authorize(Roles = "ADMIN_ORDENES")]
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
        /// Obtiene una colección de órdenes que cumplan con los filtros y parámetros.
        /// </summary>
        /// <param name="paramPagina">Los parámetros de paginado del resultado.</param>
        /// <param name="paramsOrden">Los filtros de selección para la colección de órdenes.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenesConFiltros([FromQuery] DTOParamsPagina? paramsPagina, [FromQuery] DTOParamsOrden paramsOrden)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {       
                // Obtener todas las ordenes, segun los filtros recibidos.
                var ordenes = await _repositorioOrdenes.GetOrdenes(
                    paramsPagina,
                    paramsOrden
                );

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOOrden>
                                .DesdeColeccion(ordenes, numPagina, ruta);

                return Ok(resultado);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("usuario/{idUsuario?}")]
        [Authorize(Roles = "ADMIN_ORDENES,NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenesDeUsuario(Guid? idUsuario, [FromQuery] DTOParamsPagina? paramsPagina, [FromQuery] DTOParamsOrden paramsOrden)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                paramsOrden.IdCliente = idUsuario;

                // Obtener el ID del usuario actual desde el JWT.
                Guid idUsuarioActual = new Guid(this.User.Claims.First(i => i.Type == "id").Value);
                string rolDeUsuario = this.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value;
                
                bool usuarioEsAdmin = rolDeUsuario.Equals(RolDeUsuario.ADMIN_ORDENES);

                if (!usuarioEsAdmin) 
                { 
                    paramsOrden.IdCliente = idUsuarioActual;
                    _logger.LogInformation("Usando ID del mismo usuario como cliente, no es admin");

                    bool obteniendoOrdenesAjenas = idUsuario != idUsuarioActual || paramsOrden.IdCliente != idUsuarioActual;

                    if (obteniendoOrdenesAjenas) 
                    {
                        // Si el usuario actual no es un administrador de órdenes y solicita
                        // obtener órdenes de un usuario que no es él, retornar un No Autorizado.
                        throw new UnauthorizedAccessException("Solo los administradores de órdenes pueden acceder a las órdenes de otros usuarios");
                    }
                }

                var ordenes = await _repositorioOrdenes.GetOrdenes(paramsPagina, paramsOrden);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOOrden>
                                .DesdeColeccion(ordenes, numPagina, ruta);

                return Ok(resultado);
            }
            catch (ArgumentException e)
            {
                // No existe un usuario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e) 
            {
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("{idOrden}")]
        [Authorize(Roles = "ADMIN_ORDENES,NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdenPorId(Guid idOrden)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
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

        /// <summary>
        /// Registra una nueva orden pendiente y comienza un PaymentIntent de Stripe.
        /// </summary>
        /// <param name="nuevaOrden">Una lista de IDs de productos, cada uno con su cantidad.</param>
        /// <returns>El ID de la orden y el secreto de cliente de Stripe</returns>
        [HttpPost("payment-intent")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CrearIntentDePago([FromBody] DTONuevaOrden productosOrden)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");
            
            try 
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);

                // Crear una nueva orden pendiente.
                DTOOrden ordenCreada = await _repositorioOrdenes.NuevaOrden(productosOrden, idUsuarioActual);

                // Iniciar el proceso de pago con Stripe.
                var servicioPaymentIntent = new PaymentIntentService();
                var intentDePago = servicioPaymentIntent.Create(new PaymentIntentCreateOptions
                {
                    Amount = ordenCreada.MontoTotalEnCentavos,
                    Currency = "mxn",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    },
                });

                // Retornar resultado de proceso de pedido.
                return CreatedAtAction(
                    nameof(GetOrdenPorId),
                    new { idOrden = ordenCreada.Id },
                    new {
                        clientSecret = intentDePago.ClientSecret,
                        orden = ordenCreada,
                    }
                );
            }
            catch (StripeException e) 
            {
                _logger.LogError($"Error de Stripe: {e.Message}");
                return Problem(
                    "Ocurrió un error al procesar el pago. Intenta más tarde.",
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario recibido en el JWT no es válido." + e.Message);
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
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {                
                await _repositorioOrdenes.ModificarEstadoDeOrden(idOrden, nuevoEstado);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe una orden con el ID solicitado. Retorna 404.
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

        /// <summary>
        /// Es invocado por Stripe cuando hay cambios en el estado de pago de 
        /// la orden.
        /// </summary>
        /// <remarks>
        /// Por el momento, solo cambia el estado de orden:
        /// PENDIENTE -> EN_PROGRESO.
        /// Cuando el pago fue confirmado por Stripe.
        /// </remarks>
        /// <param name="idOrden">La orden que se va a actualizar.</param>
        /// <param name="nuevoEstado">El nuevo estado para la orden.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idOrden}/stripe-hook")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarOrdenConStripe(Guid idOrden, EstadoOrden nuevoEstado)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {                
                await _repositorioOrdenes.ModificarEstadoDeOrden(idOrden, nuevoEstado);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe una orden con el ID solicitado. Retorna 404.
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
#nullable disable
