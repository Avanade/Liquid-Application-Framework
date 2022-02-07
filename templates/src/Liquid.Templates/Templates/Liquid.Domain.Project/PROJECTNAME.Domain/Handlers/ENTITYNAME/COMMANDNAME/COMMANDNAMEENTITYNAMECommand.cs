using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME
{
    public class COMMANDNAMEENTITYNAMECommand : IRequest<COMMANDNAMEENTITYNAMECommandResponse>
    {
        public ENTITYNAMEEntity Body { get; set; }

        public COMMANDNAMEENTITYNAMECommand(ENTITYNAMEEntity body)
        {
            Body = body;
        }
    }
}
