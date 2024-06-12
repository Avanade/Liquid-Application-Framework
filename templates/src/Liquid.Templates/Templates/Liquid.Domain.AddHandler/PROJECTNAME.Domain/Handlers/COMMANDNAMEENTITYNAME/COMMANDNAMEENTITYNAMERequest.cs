using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class COMMANDNAMEENTITYNAMERequest : IRequest<COMMANDNAMEENTITYNAMEResponse>
    {
        public ENTITYNAME Body { get; set; }

        public COMMANDNAMEENTITYNAMERequest(ENTITYNAME body)
        {
            Body = body;
        }
    }
}
