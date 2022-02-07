using MediatR;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete
{
    public class DeleteENTITYNAMECommand : IRequest<DeleteENTITYNAMECommandResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public DeleteENTITYNAMECommand(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
