using Liquid.Core.Exceptions;
using System;

namespace Liquid.Data.EntityFramework.Exceptions
{
    [Serializable]
    public class EntityFrameworkException : LightException
    {
        public EntityFrameworkException(Exception innerException = null) : base("An error has occurred in database command. Please see inner exception", innerException) { }
    }
}