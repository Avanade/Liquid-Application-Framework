using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class PutENTITYNAMECommand : IRequest<PutENTITYNAMECommandResponse>
    {
        public Entities.ENTITYNAME Body { get; set; }

        public PutENTITYNAMECommand(Entities.ENTITYNAME body)
        {
            Body = body;
        }
    }
}
