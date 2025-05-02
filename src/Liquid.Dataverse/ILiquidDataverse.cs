using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Liquid.Dataverse
{
    /// <summary>
    /// Dataverse integration service definition.
    /// </summary>
    public interface ILiquidDataverse
    {
        /// <summary>
        /// Insert an <see cref="Entity"/> using additional parameters to optionally prevent custom synchronous logic execution, suppress Power Automate trigger and enforce duplicate detection rules evaluation.
        /// </summary>
        /// <param name="targetEntity">entity definition.</param>
        /// <param name="bypassSynchronousCustomLogic"></param>
        /// <param name="suppressPowerAutomateTrigger"></param>
        /// <param name="suppressDuplicateDetectionRules"></param>
        /// <returns>created entity Id.</returns>
        Task<Guid> Create(Entity targetEntity, bool bypassSynchronousCustomLogic = false, bool suppressPowerAutomateTrigger = false, bool suppressDuplicateDetectionRules = true);

        /// <summary>
        /// Update an <see cref="Entity"/> record using parameters to enforce optimistic concurrency, prevent custom synchronous logic execution, suppress Power Automate trigger, and enforce duplicate detection rules evaluation.
        /// </summary>
        /// <param name="entity">entity definition.</param>
        /// <param name="useOptimisticConcurrency"></param>
        /// <param name="bypassSynchronousCustomLogic"></param>
        /// <param name="suppressPowerAutomateTrigger"></param>
        /// <param name="suppressDuplicateDetectionRules"></param>
        Task Update(Entity entity, bool useOptimisticConcurrency = false, bool bypassSynchronousCustomLogic = false, bool suppressPowerAutomateTrigger = false, bool suppressDuplicateDetectionRules = true);

        /// <summary>
        /// Read <paramref name="entityName"/> by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">primarykey value.</param>
        /// <param name="entityName">table name.</param>
        /// <param name="columns"> column set should return.</param>
        Task<Entity> GetById(Guid id, string entityName, ColumnSet? columns = null);

        /// <summary>
        /// Read table <paramref name="entityName"/> according filter conditions.
        /// </summary>
        /// <param name="entityName">table name.</param>
        /// <param name="filter">query conditions.</param>
        /// <param name="columns">conlumn se should return.</param>
        Task<List<Entity>> ListByFilter(string entityName, FilterExpression filter, ColumnSet? columns = null);

        /// <summary>
        /// Exclude an item from <paramref name="entityName"/> table by primarykey optionally using parameters to prevent custom synchronous logic execution and enforce optimistic concurrency.
        /// </summary>
        /// <param name="id">primarykey value</param>
        /// <param name="entityName">table name.</param>
        /// <param name="bypassSynchronousLogic"></param>
        /// <param name="useOptimisticConcurrency"></param>
        Task Delete(Guid id, string entityName, bool bypassSynchronousLogic = false, bool useOptimisticConcurrency = false);

        /// <summary>
        /// Read table <paramref name="entityName"/> according query conditions.
        /// </summary>
        /// <param name="entityName">table name.</param>
        /// <param name="query">query conditions</param>
        Task<List<Entity>> ListByFilter(string entityName, QueryExpression query);

        /// <summary>
        /// Read table <paramref name="entityName"/> properties.
        /// </summary>
        /// <param name="entityName">table name.</param>
        /// <returns><see cref="EntityMetadata"/> set.</returns>
        Task<EntityMetadata> GetMetadata(string entityName);

        /// <summary>
        /// Update state and status from an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">entity reference.</param>
        /// <param name="state">new state value.</param>
        /// <param name="status">new status value.</param>
        Task SetState(EntityReference entity, string state, string status);

        /// <summary>
        /// Insert or update an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">entity definition.</param>
        Task Upsert(Entity entity);

    }
}
