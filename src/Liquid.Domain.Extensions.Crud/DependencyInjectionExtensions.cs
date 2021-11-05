using Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity;
using Liquid.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Register Handlers to Perform CRUD
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TIdentifier"></typeparam>
        /// <param name="services">Instance of <see cref="IServiceCollection"/></param>
        public static void RegisterCrud<TEntity, TIdentifier>(this IServiceCollection services) where TEntity : LiquidEntity<TIdentifier>
        {
            services.TryAddTransient<IRequestHandler<AddGenericEntityCommand<TEntity, TIdentifier>, AddGenericEntityCommandResponse<TEntity>>, AddGenericEntityCommandHandler<TEntity, TIdentifier>>();
            services.TryAddTransient<IRequestHandler<FindByIdGenericEntityQuery<TEntity, TIdentifier>, FindByIdGenericEntityQueryResponse<TEntity>>, FindByIdGenericEntityQueryHandler<TEntity, TIdentifier>>();
            services.TryAddTransient<IRequestHandler<GetAllGenericEntityQuery<TEntity, TIdentifier>, GetAllGenericEntityQueryResponse<TEntity>>, GetAllGenericEntityQueryHandler<TEntity, TIdentifier>>();
            services.TryAddTransient<IRequestHandler<RemoveGenericEntityCommand<TEntity, TIdentifier>, RemoveGenericEntityCommandResponse<TEntity>>, RemoveGenericEntityCommandHandler<TEntity, TIdentifier>>();
            services.TryAddTransient<IRequestHandler<UpdateGenericEntityCommand<TEntity, TIdentifier>, UpdateGenericEntityCommandResponse<TEntity>>, UpdateGenericEntityCommandHandler<TEntity, TIdentifier>>();
        }
    }
}
