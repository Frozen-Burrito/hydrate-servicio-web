using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ServicioHydrate.Modelos;

namespace ServicioHydrate.Autenticacion
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Autorizacion : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permitirUsuarioAnonimo = context.ActionDescriptor.EndpointMetadata.OfType<PermitirAnonimo>().Any();
            if (permitirUsuarioAnonimo) return;

            var usuario = (Usuario) context.HttpContext.Items["Usuario"];
            if (usuario == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) 
                { 
                    StatusCode = StatusCodes.Status401Unauthorized 
                };
        }
    }
}