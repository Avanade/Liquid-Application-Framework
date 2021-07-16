using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Repository.EntityFramework.Extensions
{
    /// <summary>
    /// Entity Framework Service Collection Extensions Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="EntityFrameworkRepository{TEntity, TIdentifier, TContext}"/> service 
        /// for the entity <typeparamref name="TEntity"/>, and <see cref="DbContext"/> 
        /// <typeparamref name="TContext"/> with <see cref="IEntityFrameworkDataContext{TContext}"/> 
        /// if not previously registered.
        /// This method also registers <see cref="LiquidTelemetryInterceptor"/> for EntityFrameworkRepository instance.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="optionsAction">  An action to configure the <see cref="DbContextOptions"/> 
        ///  for the context.</param>
        public static IServiceCollection AddLiquidEntityFramework<TContext, TEntity, TIdentifier>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
            where TEntity : LiquidEntity<TIdentifier>, new()
            where TContext : DbContext
        {
            AddLiquidDbContext<TContext>(services, optionsAction);

            services.AddScoped<EntityFrameworkRepository<TEntity, TIdentifier, TContext>>();

            services.AddLiquidTelemetryInterceptor<ILiquidRepository<TEntity, TIdentifier>, EntityFrameworkRepository<TEntity, TIdentifier, TContext>>();

            return services;
        }

        private static void AddLiquidDbContext<TContext>(IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) where TContext : DbContext
        {
            var dbContext = services.FirstOrDefault(x => x.ServiceType == typeof(IEntityFrameworkDataContext<TContext>));

            if(dbContext is null)
            {
                services.AddDbContext<TContext>(optionsAction);

                services.AddScoped<IEntityFrameworkDataContext<TContext>, EntityFrameworkDataContext<TContext>>();                
            }            
        }
    }
}
