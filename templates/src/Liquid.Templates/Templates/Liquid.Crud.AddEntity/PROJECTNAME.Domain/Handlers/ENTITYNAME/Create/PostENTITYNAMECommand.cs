using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Create
{
    public class PostENTITYNAMECommand : IRequest
    {
        public Entities.ENTITYNAME Body { get; set; }

        public PostENTITYNAMECommand(Entities.ENTITYNAME body)
        {
            Body = body;
        }
    }
}
