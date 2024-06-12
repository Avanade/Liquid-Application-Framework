using MediatR;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class RemoveENTITYNAMERequest : IRequest<RemoveENTITYNAMEResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public RemoveENTITYNAMERequest(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
