using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;
using System.Reflection;
using System.IO;
using System.Text;
using ServicioHydrate.Modelos;
using System.Collections.Generic;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Authorize]
    [Route("api/v1/datos-abiertos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorDatosAbiertos : ControllerBase
    {
        /// El repositorio de acceso a datos abiertos.
        private readonly IServicioDatosAbiertos _repoDatosAbiertos;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorDatosAbiertos> _logger;

        public ControladorDatosAbiertos(
            IServicioDatosAbiertos servicioDatosAbiertos,
            ILogger<ControladorDatosAbiertos> logger
        )
        {
            this._repoDatosAbiertos = servicioDatosAbiertos;
            this._logger = logger;
        }

        /// <summary>
        /// Obtiene los registros de hidratación que cumplan con los filtros y 
        /// sean parte de datos abiertos.
        /// </summary>
        /// <remarks>
        /// La información es seleccionada en base a información no confidencial
        /// del perfil de usuario, tales como edad o nacionalidad.
        /// 
        /// El resultado tiene una forma general, como una colección de objetos JSON.
        /// </remarks>
        /// <param name="filtros">Los criterios que definen los registros que son incluidos.</param>
        /// <returns>Un resultado HTTP con los datos de hidratación.</returns>
        [HttpGet("hidratacion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORegistroDeHidratacion>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerDatosDeHidratacion(
            [FromQuery] FiltrosPorPerfil filtros,
            [FromQuery] DTOParamsPagina paramsPagina
        )
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var datosDeHidratacion = await _repoDatosAbiertos.GetDatosDeHidratacion(filtros, paramsPagina);
              
                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultadoPaginado = DTOResultadoPaginado<DTORegistroDeHidratacion>
                                .DesdeColeccion(datosDeHidratacion, numPagina, ruta);

                return Ok(resultadoPaginado);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene los registros de hidratación que cumplan con los filtros y 
        /// sean parte de datos abiertos.
        /// </summary>
        /// <remarks>
        /// La información es seleccionada en base a información no confidencial
        /// del perfil de usuario, tales como edad o nacionalidad.
        /// 
        /// El resultado tiene una forma dedicada para ser visualizado en una 
        /// gráfica de dos ejes.
        /// </remarks>
        /// <param name="filtros">Los criterios que definen los registros que son incluidos.</param>
        /// <returns>El DTO del perfil encontrado, o un error.</returns>
        [HttpGet("hidratacion/grafica")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultadoGrafico<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerGraficaDeHidratacion([FromQuery] FiltrosPorPerfil filtros)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener los registros de hidratación, sin paginar el resultado.
                var datosDeHidratacion = await _repoDatosAbiertos.GetDatosDeHidratacion(filtros, null);

                // Transformar los registros a un formato común para gráficas.
                ResultadoGrafico<int> resGrafico = ResultadoGrafico<int>.DesdeColeccion(datosDeHidratacion);

                return Ok(resGrafico);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene los registros de actividad física que cumplan con los filtros y 
        /// sean parte de datos abiertos.
        /// </summary>
        /// <remarks>
        /// La información es seleccionada en base a información no confidencial
        /// del perfil de usuario, tales como edad o nacionalidad.
        /// 
        /// El resultado tiene una forma general, como una colección de objetos JSON.
        /// </remarks>
        /// <param name="filtros">Los criterios que definen los registros que son incluidos.</param>
        /// <returns>Un resultado HTTP con los datos de actividad física.</returns>
        [HttpGet("act-fisica")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOActividad>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDatosDeActividadFisica(
            [FromQuery] FiltrosPorPerfil filtros,
            [FromQuery] DTOParamsPagina paramsPagina
        )
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var datosDeActividad = await _repoDatosAbiertos.GetDatosDeActividad(filtros, paramsPagina);
              
                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultadoPaginado = DTOResultadoPaginado<DTOActividad>
                                .DesdeColeccion(datosDeActividad, numPagina, ruta);

                return Ok(resultadoPaginado);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene los registros de actividad física que cumplan con los filtros y 
        /// sean parte de datos abiertos.
        /// </summary>
        /// <remarks>
        /// La información es seleccionada en base a información no confidencial
        /// del perfil de usuario, tales como edad o nacionalidad.
        /// 
        /// El resultado tiene una forma dedicada para ser visualizado en una 
        /// gráfica de dos ejes.
        /// </remarks>
        /// <param name="filtros">Los criterios que definen los registros que son incluidos.</param>
        /// <returns>Un resultado que incluye los puntos de datos de la gráfica de actividad física.</returns>
        [HttpGet("act-fisica/grafica")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultadoGrafico<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerGraficaDeActividad([FromQuery] FiltrosPorPerfil filtros)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener los registros de hidratación, sin paginar el resultado.
                var datosDeActividad = await _repoDatosAbiertos.GetDatosDeActividad(filtros, null);

                // Transformar los registros a un formato común para gráficas.
                ResultadoGrafico<int> resGrafico = ResultadoGrafico<int>.DesdeColeccion(datosDeActividad);

                return Ok(resGrafico);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Agrega los datos de actividad física a la colección de datos abiertos.
        /// </summary>
        /// <remarks>
        /// Solo aquellos usuarios que no tengan ningún rol especial (usuarios 
        /// "comunes") pueden aportar información a datos abiertos.
        /// </remarks>
        /// <param name="datos">Los datos de actividad física aportados.</param>
        /// <returns>Resultado HTTP exitoso, sin un cuerpo.</returns>
        [HttpPost("act-fisica")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> AportarDatosDeActFisica(
            IEnumerable<DTOActividad> datos
        )
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                await _repoDatosAbiertos.AportarDatosDeActividad(datos);

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
        /// Agrega los datos de hidratación a la colección de datos abiertos.
        /// </summary>
        /// <remarks>
        /// Solo aquellos usuarios que no tengan ningún rol especial (usuarios 
        /// "comunes") pueden aportar información a datos abiertos.
        /// </remarks>
        /// <param name="datos">Los datos de hidratación aportados.</param>
        /// <returns>Resultado HTTP exitoso, sin un cuerpo.</returns>
        [HttpPost("hidratacion")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> AportarDatosDeHidratacion(
            IEnumerable<DTORegistroDeHidratacion> datos
        )
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                await _repoDatosAbiertos.AportarDatosDeHidratacion(datos);

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
        /// Exporta la colección de datos abiertos deseada en formato CSV.
        /// </summary>
        /// <param name="filtros">Los criterios que deben cumplir los datos incluidos.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("exportar/csv")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportarOrdenesComoCSV(
            [FromQuery] TipoDeDatosAbiertos coleccion,
            [FromQuery] FiltrosPorPerfil filtros
        )
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {       
                var datos = await _repoDatosAbiertos.ExportarDatosAbiertos(coleccion, filtros);

                // // TEMPORAL: Un workaround para evitar un error con Writes síncronos.
                // var featureSyncIO = Response.HttpContext.Features.Get<IHttpBodyControlFeature>();
                // if (featureSyncIO is not null)
                // {
                //     featureSyncIO.AllowSynchronousIO = true;
                // }

                // Obtener el tipo y el tipo de los elementos de la colección.
                Type tipo = datos.GetType();
                Type? tipoDeElemento;

                if (tipo.GetGenericArguments().Length > 0) 
                {
                    tipoDeElemento = tipo.GetGenericArguments()[0];
                } else 
                {
                    tipoDeElemento = tipo.GetElementType();
                }

			    PropertyInfo[] propiedades = tipoDeElemento != null 
                    ? tipoDeElemento.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    : new PropertyInfo[0];
                
                Stream stream = new MemoryStream();

                // Usar un StreamWriter para generar el cuerpo de la respuesta HTTP.
                var streamWriter = new StreamWriter(stream, Encoding.UTF8);

                // Escribir los datos en formato CSV en el stream de la respuesta.
                await datos.ComoCSV(streamWriter, propiedades, true);

                await streamWriter.FlushAsync();

                stream.Seek(0, SeekOrigin.Begin);

                // string tipoDeDatos = tipo.Name

                string nombreArchivo = $"{DateTime.Now.ToString("o").Substring(0, 16)}_datos.csv";

                return File(stream, "text/csv", nombreArchivo);
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