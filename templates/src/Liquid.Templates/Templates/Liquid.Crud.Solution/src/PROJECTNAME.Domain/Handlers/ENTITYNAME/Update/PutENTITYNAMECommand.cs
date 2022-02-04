using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
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
