using FluentValidation;
using Liquid.Repository;

namespace Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity
{
    /// <summary>
    /// Command validator of <see cref="UpdateGenericEntityCommand{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public abstract class UpdateGenericEntityCommandValidator<TEntity, TIdentifier> : AbstractValidator<UpdateGenericEntityCommand<TEntity, TIdentifier>> where TEntity : LiquidEntity<TIdentifier> { }
}