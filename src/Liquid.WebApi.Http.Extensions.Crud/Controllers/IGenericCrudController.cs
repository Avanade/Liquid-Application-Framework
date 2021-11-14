using Liquid.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Extensions.Crud.Controllers
{
    /// <summary>
    /// Interface that exposes generic CRUD actions
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public interface IGenericCrudController<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Action to CREATE a new TEntity.
        /// </summary>
        /// <param name="entity">Entity to Create</param>
        /// <returns></returns>
        Task<IActionResult> AddAsync(TEntity entity);
        /// <summary>
        /// Action to LIST ALL TEntity
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> GetAllAsync();
        /// <summary>
        /// Action to GET by ID a TEntity
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<IActionResult> GetByIdAsync(TIdentifier id);
        /// <summary>
        /// Action to DELETE a TEntity
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<IActionResult> RemoveAsync(TIdentifier id);
        /// <summary>
        /// Action to UPDATE a TEntity
        /// </summary>
        /// <param name="entity">Entity to Update</param>
        /// <returns></returns>
        Task<IActionResult> UpdateAsync(TEntity entity);
    }
}