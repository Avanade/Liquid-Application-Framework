using FluentValidation;
using Liquid.Repository;

namespace Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity
{
    public abstract class UpdateGenericEntityCommandValidator<TEntity, TIdentifier> : AbstractValidator<UpdateGenericEntityCommand<TEntity, TIdentifier>> where TEntity : LiquidEntity<TIdentifier> { }
}