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
using ServicioHydrate.Modelos.Enums;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO.Datos;
using System.Collections.Generic;
using ServicioHydrate.Autenticacion;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Authorize]
    [Route("api/v1/datos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorDatos : ControllerBase
    {
        /// El repositorio de acceso a los datos de un usuario
        private readonly IServicioDatos _repoDatos;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorDatos> _logger;

        public ControladorDatos(
            IServicioDatos servicioDatos,
            ILogger<ControladorDatos> logger
        )
        {
            this._repoDatos = servicioDatos;
            this._logger = logger;
        }

        /// <summary>
        /// Obtiene las metas de hidratación establecidas por el usuario con el perfil e
        /// ID de cuenta especificados en la autenticación de la petición.
        /// </summary>
        /// <remarks>
        /// Solo es posible obtener los datos del mismo usuario autenticado. 
        /// </remarks>
        /// <returns>El resultado paginado de metas, o un error.</returns>
        [HttpGet("metas")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOMeta>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMetasDelPerfil([FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                ICollection<DTOMeta> metas = await _repoDatos.GetMetasPorPerfil(idPerfil, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOMeta>
                                .DesdeColeccion(metas, numPagina, ruta);

                return Ok(resultado);
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibidos en el JWT no son válidos." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil con el idPerfil e idUsuario especificados. Retorna 404.
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
        /// Actualiza la colección de metas de hidratación de un usuario específico.
        /// </summary>
        /// <remarks>
        /// Si una o varias de las metas incluidas en "metasActualizadas" ya existe para 
        /// el usuario, su registro es actualizado.
        /// 
        /// Solo es posible actualizar los datos del mismo usuario autenticado por 
        /// el ID de perfil e ID de cuenta de usuario encontrados en la autorización
        /// de la petición. 
        /// </remarks>
        /// <param name="metasActualizadas">El ID del perfil seleccionado por el usuario.</param>
        /// <returns>El DTO de la configuración para el perfil, o un error.</returns>
        [HttpPut("metas")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ActualizarMetasDeHidratacion([FromBody] ICollection<DTOMeta> metasActualizadas)
        {
            // Registrar un log de la petición.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                await _repoDatos.AgregarMetas(idPerfil, metasActualizadas);

                return NoContent();
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibido en el JWT no es válido." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil o usuario con los IDs especificados. Retorna 404.
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
        /// Elimina una meta de hidratación de un usuario específico.
        /// </summary>
        /// <remarks>
        /// Solo es posible eliminar los datos del mismo usuario autenticado por 
        /// el ID de perfil e ID de cuenta de usuario encontrados en la autorización
        /// de la petición. 
        /// </remarks>
        /// <param name="idMeta">El ID de la meta a eliminar.</param>
        /// <returns>Resultado HTTP confirmando la eliminación, o un error.</returns>
        [HttpDelete("metas/{idMeta}")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarMetaDeHidratacion([FromRoute] int idMeta)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                await _repoDatos.EliminarMeta(idPerfil, idMeta);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un perfil, un usuario o una meta con el ID solicitado. Retorna 404.
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
        /// Obtiene los registros de hidratación generados por el usuario con el perfil e
        /// ID de cuenta especificados en la autenticación de la petición.
        /// </summary>
        /// <remarks>
        /// Solo es posible obtener los datos del mismo usuario autenticado. 
        /// </remarks>
        /// <returns>El resultado paginado con los registros de hidratación, o un error.</returns>
        [HttpGet("hidratacion")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORegistroDeHidratacion>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegistrosDeHidratacion([FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                ICollection<DTORegistroDeHidratacion> registrosHidratacion = await _repoDatos
                    .GetHidratacionPorPerfil(idPerfil, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTORegistroDeHidratacion>
                                .DesdeColeccion(registrosHidratacion, numPagina, ruta);

                return Ok(resultado);
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibidos en el JWT no son válidos." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil con el idPerfil e idUsuario especificados. Retorna 404.
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
        /// Actualiza la colección de registros de hidratación de un usuario específico.
        /// </summary>
        /// <remarks>
        /// Si uno o varios de las registros de hidratación incluidos en "regHidratacionActualizados" ya existe para 
        /// el usuario, su registro es actualizado.
        /// 
        /// Solo es posible actualizar los datos del mismo usuario autenticado por 
        /// el ID de perfil e ID de cuenta de usuario encontrados en la autorización
        /// de la petición. 
        /// </remarks>
        /// <param name="regHidratacionActualizados">La colección de registros de hidratación por agregar/actualizar</param>
        /// <returns>Un resultado HTTP confirmando el éxito de la acción, o un error.</returns>
        [HttpPut("hidratacion")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ActualizarRegistrosDeHidratacion(
            [FromBody] ICollection<DTORegistroDeHidratacion> regHidratacionActualizados
        )
        {
            // Registrar un log de la petición.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                await _repoDatos.AgregarHidratacion(idPerfil, regHidratacionActualizados);

                //TODO: enviar una notificacion, si es necesario.
                string? messageId = await _repoDatos.NotificarAlertaBateria(idUsuarioActual, idPerfil);

                _logger.LogInformation($"Notification messageId: {messageId ?? "null"}");

                return NoContent();
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibido en el JWT no es válido." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil o usuario con los IDs especificados. Retorna 404.
                return NotFound(e.Message);
            }
            catch (DbUpdateException e)
            {
                // Hubo un error al modificar la base de datos. Enviarlo a los logs y retornar 503.
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
        /// Obtiene los registros de actividad física generados por el usuario con el perfil e
        /// ID de cuenta especificados en la autenticación de la petición.
        /// </summary>
        /// <remarks>
        /// Solo es posible obtener los datos del mismo usuario autenticado. 
        /// </remarks>
        /// <returns>El resultado paginado con los registros de actividad, o un error.</returns>
        [HttpGet("actividad-fisica")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORegistroActividad>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegistrosDeActividad([FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims
                    .FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                ICollection<DTORegistroActividad> registrosDeActividad = await _repoDatos.GetActividadesPorPerfil(idPerfil, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTORegistroActividad>
                                .DesdeColeccion(registrosDeActividad, numPagina, ruta);

                return Ok(resultado);
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibidos en el JWT no son válidos." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil con el idPerfil e idUsuario especificados. Retorna 404.
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
        /// Actualiza la colección de registros de actividad de un usuario específico.
        /// </summary>
        /// <remarks>
        /// Si uno o más registros de actividad incluidos en "registrosDeActividad" ya existe para 
        /// el usuario, su registro es actualizado.
        /// 
        /// Solo es posible actualizar los datos del mismo usuario autenticado por 
        /// el ID de perfil e ID de cuenta de usuario encontrados en la autorización
        /// de la petición. 
        /// </remarks>
        /// <param name="registrosDeActividad">Los registros de actividad que serán agregados/actualizados.</param>
        /// <returns>Un resultado HTTP confirmando la operación, o un error.</returns>
        [HttpPut("actividad-fisica")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ActualizarRegistrosDeActividad([FromBody] ICollection<DTORegistroActividad> registrosDeActividad)
        {
            // Registrar un log de la petición.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                await _repoDatos.AgregarActividadFisica(idPerfil, registrosDeActividad);

                return NoContent();
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibido en el JWT no es válido." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil o usuario con los IDs especificados. Retorna 404.
                return NotFound(e.Message);
            }
            catch (DbUpdateException e)
            {
                // Hubo un error al modificar la base de datos. Enviarlo a los logs y retornar 503.
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
        /// Obtiene las rutinas definidas  por el usuario con el perfil e
        /// ID de cuenta especificados en la autenticación de la petición.
        /// </summary>
        /// <remarks>
        /// Solo es posible obtener los datos del mismo usuario autenticado. 
        /// </remarks>
        /// <returns>El resultado paginado de rutinas, o un error.</returns>
        [HttpGet("rutinas")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORutina>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRutinasDeActividadFisica([FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                ICollection<DTORutina> rutinas = await _repoDatos.GetRutinasPorPerfil(idPerfil, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                DTOResultadoPaginado<DTORutina> resultado = DTOResultadoPaginado<DTORutina>
                                .DesdeColeccion(rutinas, numPagina, ruta);

                return Ok(resultado);
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibidos en el JWT no son válidos." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil con el idPerfil e idUsuario especificados. Retorna 404.
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
        /// Actualiza la colección de rutinas de un usuario específico.
        /// </summary>
        /// <remarks>
        /// Si una o varias de las rutinas de actividad incluidas en "rutinasActualizadas" ya existe para 
        /// el usuario, su registro es actualizado.
        /// 
        /// Solo es posible actualizar los datos del mismo usuario autenticado por 
        /// el ID de perfil e ID de cuenta de usuario encontrados en la autorización
        /// de la petición. 
        /// </remarks>
        /// <param name="rutinasActualizadas">Las rutinas que serán agregadas/actualizadas.</param>
        /// <returns>Un resultado HTTP confirmando la operación, o un error.</returns>
        [HttpPut("rutinas")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ActualizarRutinasDeActFisica([FromBody] ICollection<DTORutina> rutinasActualizadas)
        {
            // Registrar un log de la petición.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);
                
                // Obtener el ID del perfil actual desde el JWT.
                string idPerfilStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdPerfil)?.Value ?? "";
                int idPerfil = int.Parse(idPerfilStr);

                await _repoDatos.AgregarRutinas(idPerfil, rutinasActualizadas);

                return NoContent();
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario o ID de perfil recibido en el JWT no es válido." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil o usuario con los IDs especificados. Retorna 404.
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
