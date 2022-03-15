using System;

namespace ServicioHydrate.Autenticacion
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermitirAnonimo : Attribute
    {
    }
}