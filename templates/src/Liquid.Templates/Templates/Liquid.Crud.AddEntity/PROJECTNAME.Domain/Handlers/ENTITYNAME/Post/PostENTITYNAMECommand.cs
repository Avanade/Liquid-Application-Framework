using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Post
{
    public class PostENTITYNAMECommand : IRequest
    {
        public ENTITYNAMEEntity Body { get; set; }

        public PostENTITYNAMECommand(ENTITYNAMEEntity body)
        {
            Body = body;
        }
    }
}
