using FluentValidation;
using Liquid.Repository;

namespace Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity
{
    public abstract class AddGenericEntityCommandValidator<TEntity, TIdentifier> : AbstractValidator<AddGenericEntityCommand<TEntity, TIdentifier>> where TEntity : LiquidEntity<TIdentifier> { }
}
