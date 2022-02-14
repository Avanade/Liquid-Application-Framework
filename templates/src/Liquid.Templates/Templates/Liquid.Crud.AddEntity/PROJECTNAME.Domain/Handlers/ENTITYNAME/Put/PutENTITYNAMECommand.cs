using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Put
{
    public class PutENTITYNAMECommand : IRequest<PutENTITYNAMECommandResponse>
    {
        public ENTITYNAMEEntity Body { get; set; }

        public PutENTITYNAMECommand(ENTITYNAMEEntity body)
        {
            Body = body;
        }
    }
}
