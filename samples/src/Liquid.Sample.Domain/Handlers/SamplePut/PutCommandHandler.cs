﻿using Liquid.Repository;
using Liquid.Sample.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers.SamplePut
{
    public class PutCommandHandler : IRequestHandler<PutCommandRequest>
    {
        private readonly ILiquidRepository<SampleEntity, Guid> _repository;

        public PutCommandHandler(ILiquidRepository<SampleEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task Handle(PutCommandRequest request, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(new SampleEntity()
            {
                Id = Guid.Parse(request.Message.Id),
                MyProperty = request.Message.MyProperty
            });
        }
    }
}
