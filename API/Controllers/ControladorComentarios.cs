#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Utilidades;
using ServicioHydrate.Autenticacion;

namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Route("api/v1/comentarios")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorComentarios : ControllerBase
    {
        /// El repositorio de acceso a los Comentarios.
        private readonly IServicioComentarios _repoComentarios;

        /// El servicio de filtrado de contenido para los comentarios.
        private readonly IServicioFiltroContenido _filtroDeContenido;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorComentarios> _logger;

        public ControladorComentarios(
            IServicioComentarios servicioComentarios,
            IServicioFiltroContenido filtroDeContenido,
            ILogger<ControladorComentarios> logger
        )
        {
            // Asegurar que el controlador tiene una instancia del repositorio.
            if (servicioComentarios is null) 
            {
                throw new ArgumentNullException(nameof(servicioComentarios));
            }

            this._repoComentarios = servicioComentarios;
            this._filtroDeContenido = filtroDeContenido;
            this._logger = logger;
        }

        /// <summary>
        /// Retorna todos los comentarios publicados en el foro.
        /// </summary>
        /// /// <param name="idUsuarioActual">El identificador del usuario actual.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOComentario>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComentariosPublicados(Guid? idUsuarioActual, [FromQuery] DTOParamsPagina? paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {                 
                // Obtener todos los comentarios publicados disponibles.
                var comentariosPublicados = await _repoComentarios.GetComentarios(idUsuarioActual, paramsPagina, soloPublicados: true);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOComentario>
                                .DesdeColeccion(comentariosPublicados, numPagina, ruta);

                return Ok(resultado);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Busca un comentario con un Id especifico y lo retorna.
        /// </summary>
        /// <remarks>
        /// Opcionalmente, puede recibir el identificador del usuario actual. Esto
        /// permite determinar si el usuario que hace la petición ha marcado o reportado
        /// el comentario obtenido.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario buscado.</param>
        /// <param name="idUsuario">El identificador del usuario actual.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("{idComentario}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOComentario))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComentarioPorId(int idComentario, Guid? idUsuario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var comentario = await _repoComentarios.GetComentarioPorId(idComentario, idUsuario);

                return Ok(comentario);
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        [HttpGet("autor/{idAutor}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOComentario>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComentariosDeAutor(Guid idAutor, Guid? idUsuario, [FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var comentarios = await _repoComentarios.GetComentariosPorUsuario(idAutor, idUsuario, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOComentario>
                                .DesdeColeccion(comentarios, numPagina, ruta);

                return Ok(resultado);
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

        /// <summary>
        /// Retorna los comentarios que han superado el mínimo de reportes y aquellos
        /// que todavía no han sido publicados.
        /// </summary>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("pendientes")]
        [Authorize(Roles = "MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOComentario>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComentariosPendientes([FromQuery] DTOParamsPagina? paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener todos los comentarios pendientes de revision.
                var comentariosPendientes = await _repoComentarios.GetComentariosPendientes(paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOComentario>
                                .DesdeColeccion(comentariosPendientes, numPagina, ruta);

                return Ok(resultado);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Retorna los motivos de cada uno de los comentarios reportados de  
        /// un usuario.
        /// </summary>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("pendientes/usuario/{idUsuario?}")]
        [Authorize(Roles = "NINGUNO,MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOComentarioArchivado>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMotivosDeComentariosArchivados(Guid idUsuario, [FromQuery] DTOParamsPagina? paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                //TODO: Encontrar manera de hacer el idUsuario opcional.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuarioAutenticado = new Guid(idStr);

                var motivos = await _repoComentarios.GetMotivosDeComentariosArchivados(idUsuario, paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOComentarioArchivado>
                                .DesdeColeccion(motivos, numPagina, ruta);

                return Ok(resultado);
            }
            catch (ArgumentException e)
            {
                // El ID del usuario no es valido. Retorna 404.
                return NotFound(e.Message);
            }
            catch (FormatException e) 
            {
                return Unauthorized($"El ID usuario no es valido: {e.Message}");
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Archiva el comentario especificado, incluye el motivo.
        /// </summary>
        /// <remarks>
        /// Opcionalmente, puede recibir el identificador del usuario actual. Esto
        /// permite determinar si el usuario que hace la petición ha marcado o reportado
        /// el comentario obtenido.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario por archivar.</param>
        /// <param name="motivo">El motivo por el que será archivado el comentario.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/archivar")]
        [Authorize(Roles = "MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOComentarioArchivado))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ArchivarComentario(int idComentario, DTOArchivarComentario motivo)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                var motivoArchivado = await _repoComentarios.ArchivarComentario(idComentario, motivo);

                return Ok(motivoArchivado);
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e) 
            {
                // El comentario no puede ser archivado, por su estado actual.
                return Problem(
                    e.Message,
                    statusCode: StatusCodes.Status405MethodNotAllowed
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
        /// Publica un comentario previamente archivado, removiendo los reportes
        /// que haya tenido.
        /// </summary>
        /// <param name="idComentario">El ID del comentario que va a ser re-publicado.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/publicar")]
        [Authorize(Roles = "MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublicarComentarioPendiente(int idComentario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                await _repoComentarios.PublicarComentarioPendiente(idComentario);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado, o no hay un 
                // registro de archivo para el comentario. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Publica un nuevo Comentario en la colección de comentarios.
        /// <summary>
        /// Publica un nuevo Comentario en el foro.
        /// </summary>
        /// <remarks>
        /// Si el comentario tiene contenido no adecuado, el comentario será
        /// marcado como "Pendiente" y no será agregado al foro público.
        /// </remarks>
        /// <param name="nuevoComentario">El comentario a publicar</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublicarComentario(DTONuevoComentario nuevoComentario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuario = new Guid(idStr);

                _logger.LogInformation($"Id usuario: {idUsuario.ToString()}");

                bool asuntoEsApto = _filtroDeContenido.ContenidoEsApto(nuevoComentario.Asunto);
                bool contenidoEsApto = _filtroDeContenido.ContenidoEsApto(nuevoComentario.Contenido);

                bool asuntoTieneKeywords = asuntoEsApto 
                    ? _filtroDeContenido.ContenidoIncluyePalabrasClave(nuevoComentario.Asunto)
                    : false;

                bool comentarioEsAdecuado = asuntoEsApto && contenidoEsApto && asuntoTieneKeywords;

                var comentarioCreado = await _repoComentarios.AgregarNuevoComentario(nuevoComentario, idUsuario, comentarioEsAdecuado);

                return CreatedAtAction(
                    nameof(GetComentarioPorId),
                    new { idComentario = comentarioCreado.Id },
                    comentarioCreado
                );
            }
            catch (FormatException)
            {
                // EL usuario con el ID de la peticion no existe. Retornar no autorizado.
                return Unauthorized();
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

        /// Actualiza el Comentario con idComentario.
        [HttpPut("{idComentario}")]
        [Authorize(Roles = "NINGUNO,MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarComentario(int idComentario, DTOComentario comentarioModificado)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals("id"))?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                comentarioModificado.Id = idComentario;

                var comentarioActualizado = await _repoComentarios.ActualizarComentario(comentarioModificado, idUsuario);

                return Ok(comentarioActualizado);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
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

        /// Elimina un Comentario con el idComentario especificado.
        [HttpDelete("{idComentario}")]
        [Authorize(Roles = "NINGUNO,MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarComentario(int idComentario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                Guid idUsuario = new Guid(this.User.Claims.First(i => i.Type == "id").Value);

                string rol = this.User.Claims.First(i => i.Type == ClaimTypes.Role).Value;

                await _repoComentarios.EliminarComentario(idComentario, idUsuario, rol);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Unauthorized(e.Message);
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
        /// Marca o quita la marca de "útil" del comentario con idComentario.
        /// </summary>
        /// <remarks>
        /// Si el comentario no tiene está marcado como útil por el usuario, se 
        /// agregará la marca. Si ya está marcado, se removerá la marca.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario marcado.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/util")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarcarComentarioComoUtil(int idComentario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            { 
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals("id"))?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                if (!idEsValido) 
                {
                    throw new ArgumentException("El ID del usuario no tiene el formato correcto.");
                }
                
                await _repoComentarios.MarcarComentarioComoUtil(idComentario, idUsuario);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
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
        /// Agrega o remueve un reporte del comentario con idComentario.
        /// </summary>
        /// <remarks>
        /// Un comentario puede se reportado solo una vez por un usuario dado.
        /// Si el comentario no tiene un reporte del usuario, se reportará el
        /// comentario. Si lo tiene, se removerá el reporte.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario reportado.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/reportar")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReportarComentario(int idComentario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == GeneradorDeToken.TipoClaimIdUsuario)?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                if (idEsValido)
                {
                    await _repoComentarios.ReportarComentario(idComentario, idUsuario);

                    return NoContent();
                } 
                else 
                {
                    return Unauthorized();
                }
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
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
        /// Retorna todas las respuestas a un comentario especifico.
        /// </summary>
        /// <param name="idComentario">El ID del comentario</param>
        /// <param name="idUsuario">El identificador del usuario actual.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("{idComentario}/respuestas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTORespuesta>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRespuestasComentario(int idComentario, Guid? idUsuario, [FromQuery] DTOParamsPagina? paramsPagina)
        {
            // Buscar comentario requerido.
            // Si se encuentra, retornar todas las respuestas publicadas asociadas con el.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener todas las respuestas de un comentario.
                var respuestas = await _repoComentarios.GetRespuestasDeComentario(idComentario, idUsuario, paramsPagina);

                return Ok(respuestas);
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
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
        /// Retorna la respuesta con Id = idRespuesta del comentario con Id = idComentario.
        /// </summary>
        /// <param name="idComentario">El ID del comentario</param>
        /// <param name="idRespuesta">El ID de la respuesta</param>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("{idComentario}/respuestas/{idRespuesta}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuesta))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRespuestaPorId(int idComentario, int idRespuesta)
        {
            // Buscar comentario requerido.
            // Si se encuentra, retornar todas las respuestas publicadas asociadas con el.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuario = new Guid(idStr);

                // Obtener todos los comentarios publicados disponibles.
                var respuesta = await _repoComentarios.GetRespuestaPorId(idComentario, idRespuesta, (Guid) idUsuario);

                return Ok(respuesta);
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (FormatException) 
            {
                return Unauthorized();
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Agrega una nueva respuesta a un comentario.
        /// </summary>
        /// <param name="idComentario">El ID del comentario al que se responde.</param>
        /// <param name="nuevaRespuesta">EL contenido de la respuesta a agregar.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPost("{idComentario}/respuestas")]
        [Authorize(Roles = "NINGUNO,MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PublicarRespuestaDeComentario(int idComentario, DTONuevaRespuesta nuevaRespuesta)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuario = new Guid(idStr);

                bool contenidoEsApto = _filtroDeContenido.ContenidoEsApto(nuevaRespuesta.Contenido);

                //TODO: Verificar el contenido de la respuesta.
                // Si el comentario no es apto, crear el comentario y marcar "Publicado" como false.
                // Si el contenido es apto, crear el comentario y marcar "Publicado" como true.
                var respuestaCreada = await _repoComentarios.AgregarNuevaRespuesta(idComentario, nuevaRespuesta, idUsuario, contenidoEsApto);

                return CreatedAtAction(
                    nameof(GetRespuestaPorId),
                    new { idComentario = idComentario, idRespuesta = respuestaCreada.Id },
                    respuestaCreada
                );
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (FormatException) 
            {
                return Unauthorized();
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
        /// Marca o quita la marca de "útil" de la respuesta con idRespuesta.
        /// </summary>
        /// <remarks>
        /// Si la respuesta no está marcada como útil por el usuario, se 
        /// agregará la marca. Si ya está marcada, se removerá la marca.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario en el que esta la respuesta.</param>
        /// <param name="idRespuesta">El ID de la respuesta a marcar.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/respuestas/{idRespuesta}/util")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarcarRespuestaComoUtil(int idComentario, int idRespuesta)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuario = new Guid(idStr);

                await _repoComentarios.MarcarRespuestaComoUtil(idComentario, idRespuesta, idUsuario);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (FormatException)
            {
                return Unauthorized();
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
        /// Agrega o remueve un reporte de la respuesta con idComentario.
        /// </summary>
        /// <remarks>
        /// Una respuesta puede se reportada solo una vez por un usuario dado.
        /// Si la respuesta no tiene un reporte del usuario, se reportará la
        /// respuesta. Si lo tiene, se removerá el reporte.
        /// </remarks>
        /// <param name="idComentario">El ID del comentario al que pertenece la respuesta.</param>
        /// <param name="idRespuesta">El ID de la respuesta a reportar.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idComentario}/respuestas/{idRespuesta}/reportar")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReportarRespuesta(int idComentario, int idRespuesta)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuario = new Guid(idStr);

                await _repoComentarios.ReportarRespuesta(idComentario, idRespuesta, idUsuario);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (FormatException)
            {
                return Unauthorized();
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
        /// Elimina una respuesta especifica, encontrada en un comentario
        /// </summary>
        /// <param name="idComentario">El ID del comentario al que pertenece la respuesta a eliminar.</param>
        /// <param name="idRespuesta">La respuesta a eliminar</param>
        /// <returns>Resultado de Accion HTTP</returns>
        [HttpDelete("{idComentario}/respuestas/{idRespuesta}")]
        [Authorize(Roles = "NINGUNO,MODERADOR_COMENTARIOS")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarRespuesta(int idComentario, int idRespuesta)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? String.Empty;
                Guid idUsuario = new Guid(idStr);

                string rol = this.User.Claims.First(i => i.Type == ClaimTypes.Role).Value;

                await _repoComentarios.EliminarRespuesta(idComentario, idRespuesta, (Guid) idUsuario, rol);

                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe una respuesta o comentario con el ID solicitado. Retorna 404.
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