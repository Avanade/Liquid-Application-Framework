using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Messaging.Tests.Mock
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
