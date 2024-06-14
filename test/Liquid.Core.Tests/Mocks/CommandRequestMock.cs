using MediatR;

namespace Liquid.Core.Tests.Mocks
{
    public class CommandRequestMock : IRequest
    {
        public EntityMock Entity { get; set; }

        public CommandRequestMock(EntityMock entity)
        {
            Entity = entity;
        }
    }
}
