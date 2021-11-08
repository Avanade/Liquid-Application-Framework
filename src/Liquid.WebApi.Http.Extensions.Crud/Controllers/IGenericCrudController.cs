using Liquid.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LiquidCrudExample.WebApi.Controllers
{
    public interface IGenericCrudController<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Action to CREATE a new <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity">Entity to Create</param>
        /// <returns></returns>
        Task<IActionResult> AddAsync(TEntity entity);
        /// <summary>
        /// Action to LIST ALL <see cref="TEntity"/>
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> GetAllAsync();
        /// <summary>
        /// Action to GET by ID a <see cref="TEntity"/>
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<IActionResult> GetByIdAsync(TIdentifier id);
        /// <summary>
        /// Action to DELETE a <see cref="TEntity"/>
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<IActionResult> RemoveAsync(TIdentifier id);
        /// <summary>
        /// Action to UPDATE a <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity">Entity to Update</param>
        /// <returns></returns>
        Task<IActionResult> UpdateAsync(TEntity entity);
    }
}