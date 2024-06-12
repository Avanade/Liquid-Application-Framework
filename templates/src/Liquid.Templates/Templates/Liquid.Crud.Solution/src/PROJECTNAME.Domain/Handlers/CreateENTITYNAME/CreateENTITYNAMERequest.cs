using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class CreateENTITYNAMERequest : IRequest
    {
        public ENTITYNAME Body { get; set; }

        public CreateENTITYNAMERequest(ENTITYNAME body)
        {
            Body = body;
        }
    }
}
