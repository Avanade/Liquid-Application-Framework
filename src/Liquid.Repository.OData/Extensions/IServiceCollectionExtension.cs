using Liquid.Core.Entities;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.OData.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="ODataRepository{TEntity, TIdentifier}"/> for the entity <typeparamref name="TEntity"/>,
        /// and a <see cref="ODataClientFactory"/>  if not previously registered.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that the repository should correspond to</typeparam>
        /// <typeparam name="TIdentifier">Entity identifier type.</typeparam>
        /// <param name="services">Extended ServiceCollection object.</param>
        /// <param name="sectionName">Name of the configuration section where all entities have their repository settings configured.</param>
        /// <param name="entityName">Name of the entity in the database that the repository should correspond to.</param>        
        public static IServiceCollection AddLiquidOdataRepository<TEntity, TIdentifier>(this IServiceCollection services, string sectionName, string entityName)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            services.TryAddSingleton<IODataClientFactory, ODataClientFactory>();
            services.TryAddSingleton<ILiquidContext, LiquidContext>();

            services.AddOptions<ODataOptions>()
             .Configure<IConfiguration>((settings, configuration) =>
             {
                 configuration.GetSection(sectionName).Bind(settings);
             });

            services.AddScoped<ILiquidRepository<TEntity, TIdentifier>>((provider) =>
            {
                return ActivatorUtilities.CreateInstance<ODataRepository<TEntity, TIdentifier>>(provider, entityName);
            });

            return services;
        }
    }
}
