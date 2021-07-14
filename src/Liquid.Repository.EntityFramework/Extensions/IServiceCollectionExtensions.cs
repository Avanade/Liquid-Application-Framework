using Liquid.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.EntityFramework.Extensions
{
    /// <summary>
    /// Entity Framework Service Collection Extensions Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the entity framework database repositories and <see cref="DbContext"/>.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="optionsAction">  An action to configure the <see cref="DbContextOptions"/> 
        ///  for the context.</param>
        public static IServiceCollection AddLiquidEntityFramework<TContext, TEntity, TIdentifier>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
            where TEntity : LiquidEntity<TIdentifier>, new()
            where TContext : DbContext
        {
            //TODO: if para não registrar mais de um dbcontext igual
            services.AddScoped<IEntityFrameworkDataContext<TContext>, EntityFrameworkDataContext<TContext>>();

            services.AddDbContext<TContext>(optionsAction);

            services.AddScoped<EntityFrameworkRepository<TEntity, TIdentifier, TContext>>();

            services.AddLiquidInterceptors<ILiquidRepository<TEntity, TIdentifier>, EntityFrameworkRepository<TEntity, TIdentifier, TContext>>();

            return services;
        }
    }
}
