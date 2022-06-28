using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using System.IO;

using ServicioHydrate.Modelos;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ServicioHydrate.Formatters
{
	/// <summary>
	/// Produce una versión en formato CSV de una colección enumerable de datos 
	/// enviada como resultado por una acción de un controlador de API.
	/// 
	/// Su tipo de contenido es "text/csv".
	/// </summary>
  	public class CsvMediaFormatter : OutputFormatter
	{
		// El tipo de contenido producido por el formatter.
		public string ContentType { get; private set; }

		public Encoding FileEncoding { get; private set; }

		public string DelimitadorCsv { get; private set; }

		public CsvMediaFormatter()
		{
			// Inicializar configuración básica.
			ContentType = "text/csv";
			FileEncoding = Encoding.UTF8;
			DelimitadorCsv = ",";

			// Especificar los tipos de contenido soportados por este formatter.
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
		}

		protected override bool CanWriteType(Type type)
		{
			if (type is null)
			{
				throw new ArgumentNullException("type");
			}

			return typeof(IEnumerable).IsAssignableFrom(type);
		}

		public override IReadOnlyList<string> GetSupportedContentTypes(string contentType, Type objectType)
		{
			return base.GetSupportedContentTypes(contentType, objectType);
		}

    	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
		{
			var contextoHttp = context.HttpContext;
			var respuesta = contextoHttp.Response;

			var serviceProvider = contextoHttp.RequestServices;

			var logger = serviceProvider.GetRequiredService<ILogger<CsvMediaFormatter>>();

			// TEMPORAL: Un workaround para evitar un error con Writes síncronos.
			var featureSyncIO = contextoHttp.Features.Get<IHttpBodyControlFeature>();
			if (featureSyncIO is not null)
			{
				featureSyncIO.AllowSynchronousIO = true;
			}

			// Obtener el tipo y el tipo de los elementos de la colección.
			Type tipo = context.Object.GetType();
			Type tipoDeElemento;

			if (tipo.GetGenericArguments().Length > 0) 
			{
				tipoDeElemento = tipo.GetGenericArguments()[0];
			} else 
			{
				tipoDeElemento = tipo.GetElementType();
			}

			PropertyInfo[] propiedades = tipoDeElemento.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// TEMPORAL: mostrar propiedades para asegurar que son correctas.
			foreach(var prop in propiedades)
			{
				logger.LogInformation($"Nombre de la propiedad: {prop.Name}, es IEnumerable: {prop.PropertyType.IsNonStringEnumerable()}");
			}

			// Usar un StreamWriter para generar el cuerpo de la respuesta HTTP.
			using (var streamWriter = new StreamWriter(respuesta.Body, FileEncoding))
			{
				// Obtener la colección de datos producida por el controlador.
				IEnumerable<object> datos = context.Object as IEnumerable<object>;

				// Escribir los datos en formato CSV en el stream de la respuesta.
				await datos.ComoCSV(streamWriter, propiedades, true);

				await streamWriter.FlushAsync();
			}
		}
	}
}