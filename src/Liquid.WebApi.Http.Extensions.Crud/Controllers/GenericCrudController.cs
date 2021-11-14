using Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity;
using Liquid.Repository;
using Liquid.WebApi.Http.Attributes;
using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Extensions.Crud.Controllers
{
    ///<inheritdoc/>
    [ApiController]
    [SwaggerCultureHeader]
    [Route("api/[controller]")]
    public class GenericCrudController<TEntity, TIdentifier> : LiquidControllerBase, IGenericCrudController<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>
    {
        ///<inheritdoc/>
        public GenericCrudController(IMediator mediator) : base(mediator) { }

        ///<inheritdoc/>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerCultureHeader]
        public async virtual Task<IActionResult> AddAsync(TEntity entity)
        {
            await ExecuteAsync(new AddGenericEntityCommand<TEntity, TIdentifier>(entity));

            return CreatedAtRoute("", new { id = entity.Id }, entity);
        }

        ///<inheritdoc/>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerCultureHeader]
        public async Task<IActionResult> GetByIdAsync(TIdentifier id)
        {
            var response = await ExecuteAsync(new FindByIdGenericEntityQuery<TEntity, TIdentifier>(id));

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        ///<inheritdoc/>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerCultureHeader]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await ExecuteAsync(new GetAllGenericEntityQuery<TEntity, TIdentifier>());

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        ///<inheritdoc/>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerCultureHeader]
        public async Task<IActionResult> RemoveAsync(TIdentifier id)
        {
            var response = await ExecuteAsync(new RemoveGenericEntityCommand<TEntity, TIdentifier>(id));

            if (response.Data == null) return NotFound();

            return NoContent();
        }

        ///<inheritdoc/>s
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerCultureHeader]
        public async Task<IActionResult> UpdateAsync(TEntity entity)
        {
            var response = await ExecuteAsync(new UpdateGenericEntityCommand<TEntity, TIdentifier>(entity));

            if (response.Data == null) return NotFound();

            return NoContent();
        }
    }
}