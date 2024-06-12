using MediatR;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class ReadENTITYNAMERequest : IRequest<ReadENTITYNAMEResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public ReadENTITYNAMERequest(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
