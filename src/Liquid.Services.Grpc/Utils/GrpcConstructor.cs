using System;
using System.Linq;
using System.Linq.Expressions;

namespace Liquid.Services.Grpc.Utils
{
    /// <summary>
    /// Constructor Delegate to create Grpc channels.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    internal delegate object ConstructorDelegate(params object[] args);

    /// <summary>
    /// 
    /// </summary>
    internal class GrpcConstructor
    {
        /// <summary>
        /// Creates the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
        {
            var constructorInfo = type.GetConstructor(parameters);

            var paramExpr = Expression.Parameter(typeof(object[]));

            var constructorParameters = parameters.Select((paramType, index) =>
                Expression.Convert(
                    Expression.ArrayAccess(paramExpr, Expression.Constant(index)), paramType)).ToArray();

            var body = Expression.New(constructorInfo, constructorParameters);

            var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
            return constructor.Compile();
        }
    }
}