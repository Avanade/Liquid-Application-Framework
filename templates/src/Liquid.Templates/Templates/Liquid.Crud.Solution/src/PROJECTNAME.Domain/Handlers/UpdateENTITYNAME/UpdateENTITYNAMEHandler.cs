using Liquid.Core.Interfaces;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class UpdateENTITYNAMEHandler : IRequestHandler<UpdateENTITYNAMERequest, UpdateENTITYNAMEResponse>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public UpdateENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }


        public async Task<UpdateENTITYNAMEResponse> Handle(UpdateENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Body.Id);

            if (data != null)
            {
                await _repository.UpdateAsync(request.Body);
            }

            return new UpdateENTITYNAMEResponse(request.Body);
        }
    }
}
