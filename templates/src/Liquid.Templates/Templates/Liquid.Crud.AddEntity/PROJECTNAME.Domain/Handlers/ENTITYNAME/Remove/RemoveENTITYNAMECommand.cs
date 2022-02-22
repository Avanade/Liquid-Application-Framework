using MediatR;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Remove
{
    public class RemoveENTITYNAMECommand : IRequest<RemoveENTITYNAMECommandResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public RemoveENTITYNAMECommand(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
