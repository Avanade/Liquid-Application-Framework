using FluentValidation;
using Liquid.Repository;

namespace Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity
{
    /// <summary>
    /// Command validator of <see cref="AddGenericEntityCommand{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public abstract class AddGenericEntityCommandValidator<TEntity, TIdentifier> : AbstractValidator<AddGenericEntityCommand<TEntity, TIdentifier>> where TEntity : LiquidEntity<TIdentifier> { }
}
