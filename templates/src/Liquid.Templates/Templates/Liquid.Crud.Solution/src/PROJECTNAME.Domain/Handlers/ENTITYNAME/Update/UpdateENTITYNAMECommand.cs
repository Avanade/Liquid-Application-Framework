using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class UpdateENTITYNAMECommand : IRequest<UpdateENTITYNAMECommandResponse>
    {
        public ENTITYNAMEEntity Body { get; set; }

        public UpdateENTITYNAMECommand(ENTITYNAMEEntity body)
        {
            Body = body;
        }
    }
}
