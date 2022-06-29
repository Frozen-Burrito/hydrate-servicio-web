using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
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
using ServicioHydrate.Modelos.Enums;

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
                // _logger.LogInformation(paramsOrden.IdCliente + ", " + paramsOrden.EmailCliente);

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

        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOStatsOrdenes))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstadisticasOrdenes()
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var resumenOrdenes = await _repositorioOrdenes.GetStatsOrdenes();

                return Ok(resumenOrdenes);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene una colección de órdenes que cumplan con los filtros y parámetros.
        /// </summary>
        /// <param name="paramPagina">Los parámetros de paginado del resultado.</param>
        /// <param name="paramsOrden">Los filtros de selección para la colección de órdenes.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("exportar/csv")]
        // [Produces("text/csv")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportarOrdenesComoCSV()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {       
                var ordenes = await _repositorioOrdenes.ExportarTodasLasOrdenes();

                // // TEMPORAL: Un workaround para evitar un error con Writes síncronos.
                // var featureSyncIO = Response.HttpContext.Features.Get<IHttpBodyControlFeature>();
                // if (featureSyncIO is not null)
                // {
                //     featureSyncIO.AllowSynchronousIO = true;
                // }

                // Obtener el tipo y el tipo de los elementos de la colección.
                Type tipo = ordenes.GetType();
                Type tipoDeElemento;

                if (tipo.GetGenericArguments().Length > 0) 
                {
                    tipoDeElemento = tipo.GetGenericArguments()[0];
                } else 
                {
                    tipoDeElemento = tipo.GetElementType();
                }

			    PropertyInfo[] propiedades = tipoDeElemento.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                Stream stream = new MemoryStream();

                // Usar un StreamWriter para generar el cuerpo de la respuesta HTTP.
                var streamWriter = new StreamWriter(stream, Encoding.UTF8);

                // Escribir los datos en formato CSV en el stream de la respuesta.
                await ordenes.ComoCSV(streamWriter, propiedades, true);

                await streamWriter.FlushAsync();

                stream.Seek(0, SeekOrigin.Begin);

                string nombreArchivo = $"{DateTime.Now.ToString("o").Substring(0, 16)}_ordenes.csv";

                return File(stream, "text/csv", nombreArchivo);
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
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "IdOrden", ordenCreada.Id.ToString() },
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
        [HttpPatch("webhooks/pagos-stripe")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarOrdenConStripe()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string secretoEndpoint = "secreto-webhook-pruebas";

            try
            {           
                var eventoStripe = EventUtility.ParseEvent(json);
                var headerFirma = Request.Headers["Stripe-Signature"];

                eventoStripe = EventUtility.ConstructEvent(json, headerFirma, secretoEndpoint);

                if (eventoStripe.Type == Events.PaymentIntentSucceeded) 
                {
                    var intentDePago = eventoStripe.Data.Object as PaymentIntent;

                    if (intentDePago is not null)
                    {
                        // Obtener el ID de la orden de los metadatos del PaymentIntent.
                        Guid idOrden = new Guid(intentDePago.Metadata["IdOrden"]);

                        _logger.LogInformation($"Pago exitoso por ${intentDePago.Amount}, ID de la orden: {idOrden}");

                        // Método para manejar el evento de Stripe.
                        _repositorioOrdenes.ModificarEstadoDeOrden(idOrden, EstadoOrden.EN_PROGRESO);
                    }
                } 
                else if (eventoStripe.Type == Events.PaymentMethodAttached)
                {
                    var metodoDePago = eventoStripe.Data.Object as PaymentMethod;

                    _logger.LogInformation("Evento de metodo de pago recibido de Stripe.");
                }
                else 
                {
                    _logger.LogWarning($"Tipo de evento no manejado: {eventoStripe.Type}");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                // Hubo un error en el proceso de pago de Stripe, retorna 400.
                _logger.LogWarning($"Error de Stripe al procesar webhook: {e.Message}");

                return BadRequest();
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"{metodo} - {ruta}: Ocurrió un error inesperado en un webhook de Stripe, {e.Message}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }    
        }
    }
}
#nullable disable
