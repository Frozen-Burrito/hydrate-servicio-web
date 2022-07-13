using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;
using System.Text.Json;

namespace ServicioHydrate.Modelos
{
    /// Esta clase proporciona métodos de utilidad para facilitar la 
    /// conversión entre objetos de modelo y DTOs.
    public static class Extensiones
    {
        public static string VerificarStrISO8601(string strFecha)
        {
            DateTime fecha;

            bool strISO8601Valido = DateTime.TryParse(strFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (strISO8601Valido)
            {
                return fecha.ToString("O");
            }
            else 
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }
        }

        /// <summary>
        /// Obtiene el ID de un Usuario desde el CLaim con llave "id" de un JWT.
        /// </summary>
        /// <param name="jwt">El Json Web Token</param>
        /// <returns>El ID del Usuario, o null si el JWT no es válido.</returns>
        public static Guid? GetIdUsuarioDesdeJwt(string jwt) 
        {
            string[] partesDelJwt = jwt.Split(".");

            if (partesDelJwt.Length != 3)
            {
                // El JWT debe tener un encabezado, un payload y una firma de verificación,
                // separados por puntos ".". Si no tiene exactamente estos elementos, el
                // JWT no tiene formato válido.
                return null;
            }

            string strClaims = partesDelJwt[1].Replace("-", "+").Replace("_", "/");

            switch (strClaims.Length % 4) {
                case 0:
                    break;
                case 2:
                    strClaims += "==";
                    break;
                case 3:
                    strClaims += "=";
                    break;
                default: 
                    // Si la longitud de uno de los strings para claims no es mod. 4,
                    // el JWT tiene un formato incorrecto.
                    return null;
            }

            byte[] bytesDecodificados = Convert.FromBase64String(strClaims);
            string claimsDecodificadas = System.Text.Encoding.UTF8.GetString(bytesDecodificados);

            var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(claimsDecodificadas);

            if (claims is null || !claims.ContainsKey("id"))
            {
                // No se pudieron deserializar correctamente los claims del token.
                return null;
            }

            bool esGuidValido = Guid.TryParse(claims["id"].ToString(), out Guid idUsuario);

            return esGuidValido ? idUsuario : null;
        }

        public static Usuario ComoModelo(this DTOPeticionAutenticacion usuario, string hashContrasenia, bool generarGUID = false)
        {
            // Lanzar una excepción de argumento si la contraseña y el hash son iguales.
            if (usuario.Password.Equals(hashContrasenia))
            {
                throw new ArgumentException("La contraseña y el hash de la contraseña no deben ser iguales.");
            }

            return new Usuario 
            {
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Password = hashContrasenia,
                PerfilDeUsuario = new Perfil(),
            };
        }

        public static DTOUsuario ComoDTO(this Usuario usuario)
        {
            return new DTOUsuario 
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                RolDeUsuario = usuario.RolDeUsuario
            };
        }

        public static Usuario ComoModelo(this DTOUsuario dtoUsuario)
        {
            return new Usuario
            {
                Id = dtoUsuario.Id,
                Email = dtoUsuario.Email,
                NombreUsuario = dtoUsuario.NombreUsuario,
            };
        }

        public static RecursoInformativo ComoModelo(this DTORecursoInformativo dtoRecurso)
        {
            return new RecursoInformativo
            {
                Id = dtoRecurso.Id,
                Titulo = dtoRecurso.Titulo,
                Url = dtoRecurso.Url,
                Descripcion = dtoRecurso.Descripcion,
                FechaPublicacion = VerificarStrISO8601(dtoRecurso.FechaPublicacion),
            };
        }

        public static DTORecursoInformativo ComoDTO(this RecursoInformativo recurso)
        {
            return new DTORecursoInformativo
            {
                Id = recurso.Id,
                Titulo = recurso.Titulo,
                Url = recurso.Url,
                Descripcion = recurso.Descripcion,
                FechaPublicacion = VerificarStrISO8601(recurso.FechaPublicacion),
            };
        }

        public static void Actualizar(this RecursoInformativo recurso, DTORecursoInformativo modificaciones)
        {
            recurso.Titulo = modificaciones.Titulo;
            recurso.Url = modificaciones.Url;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.FechaPublicacion = VerificarStrISO8601(modificaciones.FechaPublicacion);
        }

        public static Comentario ComoNuevoModelo(this DTONuevoComentario nuevoComentario, Usuario autor)
        {
            //TODO: Verificar el contenido del comentario
            // Si el comentario no es apto, crear el comentario y marcar "Publicado" como false.
            // Si el contenido es apto, crear el comentario y marcar "Publicado" como true.
            bool contenidoAdecuado = true;

            return new Comentario
            {
                Asunto = nuevoComentario.Asunto,
                Contenido = nuevoComentario.Contenido,
                Autor = autor,
                Fecha = DateTime.Now.ToString("o"),
                Publicado = contenidoAdecuado,
                UtilParaUsuarios = new List<Usuario>(),
                ReportesDeUsuarios = new List<Usuario>(),
                Respuestas = new List<Respuesta>(),
            };
        }

        public static DTOComentario ComoDTO(this Comentario comentario, Guid? idUsuarioActual)
        {
            int numeroDeRespuestas = 0;
            bool reportadoPorUsuarioActual = false;
            bool utilParaUsuarioActual = false;

            // Verificar directamente si el usuario ha marcado como util o reportado 
            // el comentario, para evitar pasar al cliente toda la lista de usuarios que han
            // hecho algo con el comentario.
            if (idUsuarioActual is not null)
            {
                if (comentario.ReportesDeUsuarios.Count > 0)
                {
                    var usuarioEnReportes = comentario.ReportesDeUsuarios.Where(u => u.Id == idUsuarioActual).FirstOrDefault();
                    
                    // Si existe un usuario en los ReportesDeUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido reportado por el usuario.
                    reportadoPorUsuarioActual = (usuarioEnReportes is not null);
                }

                if (comentario.UtilParaUsuarios.Count > 0)
                {
                    var usuarioEnUtil = comentario.UtilParaUsuarios.Where(u => u.Id == idUsuarioActual).FirstOrDefault();

                    // Si existe un usuario en UtilParaUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido marcado como util por el usuario.
                    utilParaUsuarioActual = (usuarioEnUtil is not null);
                }
            }

            if (comentario.Respuestas is not null) 
            {
                numeroDeRespuestas = comentario.Respuestas.Count;
            }

            return new DTOComentario
            {
                Id = comentario.Id,
                Asunto = comentario.Asunto,
                Contenido = comentario.Contenido,
                IdAutor = comentario.Autor.Id,
                //TODO: Usar el nombre completo del perfil del usuario autor.
                NombreAutor = comentario.Autor.NombreUsuario,
                NumeroDeRespuestas = numeroDeRespuestas,
                Fecha = VerificarStrISO8601(comentario.Fecha),
                Publicado = comentario.Publicado,
                NumeroDeReportes = comentario.ReportesDeUsuarios.Count,
                ReportadoPorUsuarioActual = reportadoPorUsuarioActual,
                NumeroDeUtil = comentario.UtilParaUsuarios.Count,
                UtilParaUsuarioActual = utilParaUsuarioActual,
            };
        }

        public static void Actualizar(this Comentario modelo, DTOComentario modificaciones)
        {
            modelo.Asunto = modificaciones.Asunto;
            modelo.Contenido = modificaciones.Contenido;
            modelo.Publicado = modificaciones.Publicado;
            modelo.Fecha = VerificarStrISO8601(modificaciones.Fecha);
        } 

        public static DTOComentarioArchivado ComoDTO(this ComentarioArchivado modelo)
        {
            return new DTOComentarioArchivado
            {
                Id = modelo.Id,
                IdComentario = modelo.IdComentario,
                Motivo = modelo.Motivo,
                Fecha = modelo.Fecha,
            };
        }

        public static ComentarioArchivado ComoModelo(this DTOArchivarComentario comentarioArchivado, int idComentario)
        {
            return new ComentarioArchivado
            {
                IdComentario = idComentario,
                Motivo = comentarioArchivado.Motivo,
                Fecha = DateTime.Now.ToString("o"),
            };
        }

        public static Respuesta ComoNuevoModelo(this DTONuevaRespuesta nuevaRespuesta, Comentario comentarioOriginal, Usuario autor)
        {
            bool contenidoAdecuado = true;

            return new Respuesta
            {
                Contenido = nuevaRespuesta.Contenido,
                Autor = autor,
                Fecha = DateTime.Now.ToString("o"),
                Publicado = contenidoAdecuado,
                UtilParaUsuarios = new List<Usuario>(),
                ReportesDeUsuarios = new List<Usuario>(),
                Comentario = comentarioOriginal,
                IdComentario = comentarioOriginal.Id,
            };
        }

        public static DTORespuesta ComoDTO(this Respuesta respuesta, Guid? idUsuarioActual)
        {
            bool reportadoPorUsuarioActual = false;
            bool utilParaUsuarioActual = false;

            // Verificar directamente si el usuario ha marcado como util o reportado 
            // el comentario, para evitar pasar al cliente toda la lista de usuarios que han
            // hecho algo con el comentario.
            if (idUsuarioActual is not null)
            {
                if (respuesta.ReportesDeUsuarios.Count > 0)
                {
                    var usuarioEnReportes = respuesta.ReportesDeUsuarios.Where(u => u.Id == idUsuarioActual).First();
                    
                    // Si existe un usuario en los ReportesDeUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido reportado por el usuario.
                    reportadoPorUsuarioActual = (usuarioEnReportes is not null);
                }

                if (respuesta.UtilParaUsuarios.Count > 0)
                {
                    var usuarioEnUtil = respuesta.UtilParaUsuarios.Where(u => u.Id == idUsuarioActual).First();

                    // Si existe un usuario en UtilParaUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido marcado como util por el usuario.
                    utilParaUsuarioActual = (usuarioEnUtil is not null);
                }
            }

            return new DTORespuesta
            {
                Id = respuesta.Id,
                Contenido = respuesta.Contenido,
                Fecha = respuesta.Fecha,
                Publicada = respuesta.Publicado,
                IdAutor = respuesta.Autor.Id,
                //TODO: Usar el nombre completo del perfil del usuario autor.
                NombreAutor = respuesta.Autor.NombreUsuario,
                NumeroDeReportes = respuesta.ReportesDeUsuarios.Count,
                NumeroDeUtil = respuesta.UtilParaUsuarios.Count,
                ReportadaPorUsuarioActual = reportadoPorUsuarioActual,
                UtilParaUsuarioActual = utilParaUsuarioActual
            };
        }

        public static DTOOrden ComoDTO(this Orden orden)
        {
            decimal montoTotal = orden.Productos.Sum(p => p.Cantidad * p.Producto.PrecioUnitario);

            List<DTOProductoCantidad> productosOrden = orden.Productos
                .Select(po => po.ComoProductoConCantidad())
                .ToList();

            string nombre = orden.Cliente.PerfilDeUsuario is not null 
                ? orden.Cliente.PerfilDeUsuario.NombreCompleto 
                : orden.Cliente.NombreUsuario;

            return new DTOOrden
            {
                Id = orden.Id,
                Estado = orden.Estado,
                Fecha = VerificarStrISO8601(orden.Fecha),
                MontoTotal = montoTotal,
                IdCliente = orden.Cliente.Id,
                NombreCliente = nombre,
                EmailCliente = orden.Cliente.Email,
                Productos = productosOrden,
            };
        }

        public static Orden ComoNuevoModelo(this DTONuevaOrden nuevaOrden, Usuario cliente)
        {
            return new Orden
            {
                Estado = EstadoOrden.PENDIENTE,
                Fecha = DateTime.Now.ToString("o"),
                Cliente = cliente,
                Productos = new List<ProductosOrdenados>(),
            };
        }

        public static DTOProducto ComoDTO(this Producto producto)
        {
            return new DTOProducto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Disponibles = producto.Disponibles,
                PrecioUnitario = producto.PrecioUnitario,
                UrlImagen = producto.UrlImagen
            };
        }

        public static Producto ComoNuevoProducto(this DTONuevoProducto nuevoProducto)
        {
            return new Producto
            {
                Nombre = nuevoProducto.Nombre,
                PrecioUnitario = nuevoProducto.PrecioUnitario,
                Descripcion = nuevoProducto.Descripcion,
                Disponibles = nuevoProducto.Disponibles,
                UrlImagen = nuevoProducto.UrlImagen,
                OrdenesDelProducto = new List<ProductosOrdenados>()
            };
        }

        public static DTOProductoCantidad ComoProductoConCantidad(this ProductosOrdenados productoOrdenado)
        {
            return new DTOProductoCantidad
            {
                IdProducto = productoOrdenado.IdProducto,
                Cantidad = productoOrdenado.Cantidad,
            };
        }

#nullable enable
        public static async Task ComoCSV(
            this IEnumerable datos, 
            StreamWriter output, 
            PropertyInfo[] propiedades, 
            bool incluirFilaHeader = false
        )
        {
            await GenerarCSV(datos, output, propiedades, incluirFilaHeader);
        }

        /// <summary>
        /// Convierte una colección enumerable de datos en un stream con formato CSV. 
        /// </summary>
        /// <remarks>
        /// El stream producido puede ser enviado al stream del cuerpo de una respuesta HTTP. 
        /// </remarks>
        /// <param name="datos">La colección de datos.</param>
        /// <param name="output"></param>
        /// <param name="propiedades">Las propiedades de un elemento de los datos, usados para los encabezados.</param>
        /// <param name="incluirFilaHeader">Si es true, la primera fila tendrá los nombres de las propiedades.</param>
        /// <returns></returns>
        public static async Task GenerarCSV(
            IEnumerable datos, 
            StreamWriter output, 
            PropertyInfo[] propiedades,
            bool incluirFilaHeader = false
        )
        {
            const string separador = ",";

            if (incluirFilaHeader) 
            {
                StringBuilder sbPropiedades = new StringBuilder();

                foreach (var propiedad in propiedades)
                {
                    string nombreColumna = propiedad.Name;

                    // Agregar comillas al nombre de la columna, si contiene
                    // el separador.
                    if (nombreColumna.Contains(separador))
                    {
                        nombreColumna = $"\"{nombreColumna}\"";
                    }

                    sbPropiedades.Append(nombreColumna);
                    sbPropiedades.Append(separador);
                }

                await output.WriteLineAsync(sbPropiedades.ToString());
            }

            foreach (var elemento in datos)
            {
                var valoresFila = elemento.GetType().GetProperties()
                    .Select(t => new 
                    {
                        Valor = t.GetValue(elemento, null),
                        TipoDePropiedad = t.PropertyType,
                    });

                StringBuilder sb = new StringBuilder();

                foreach (var celda in valoresFila)
                {
                    string valorCelda = string.Empty;

                    if (celda.Valor is not null)
                    {   
                        if (celda.TipoDePropiedad.IsNonStringEnumerable())
                        {
                            var sbColeccion = new StringBuilder();

                            var valoresDeColeccion = celda.Valor as IEnumerable;

                            if (valoresDeColeccion is not null)
                            {
                                foreach (object? elemColeccion in valoresDeColeccion)
                                {
                                    string strValorCelda = elemColeccion.ToString() ?? string.Empty;

                                    sbColeccion.AppendFormat("{0} | ", strValorCelda);
                                }
                            }


                            valorCelda = string.Concat("\"", sbColeccion.ToString(), "\"");
                        } else 
                        {
                            valorCelda = celda.Valor.ToString() ?? string.Empty;

                            valorCelda = valorCelda.Replace("\n", " ");
                            valorCelda = valorCelda.Replace("\r", " ");

                            // Agregar comillas al valor de la celda, si el valor
                            // incluye el caracter delimitador del csv (,).
                            if(valorCelda.Contains(separador))
                            {
                                valorCelda = string.Concat('\"', valorCelda, '\"');
                            }
                        }
                    }

                    sb.Append(valorCelda);
                    sb.Append(separador);
                }

                string filaSinDelimitadorFinal = sb.ToString().TrimEnd(separador.ToCharArray());

                await output.WriteLineAsync(filaSinDelimitadorFinal);
            } 
        }

        public static bool IsNonStringEnumerable(this Type tipo)
        {
            if (tipo is null || tipo == typeof(string))
            {
                return false;
            }

            return typeof(IEnumerable).IsAssignableFrom(tipo);
        }

#nullable disable
    }
}