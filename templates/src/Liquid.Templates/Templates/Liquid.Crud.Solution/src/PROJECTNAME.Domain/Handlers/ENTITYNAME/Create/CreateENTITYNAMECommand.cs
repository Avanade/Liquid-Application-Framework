using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Create
{
    public class CreateENTITYNAMECommand : IRequest
    {
        public ENTITYNAMEEntity Body { get; set; }

        public CreateENTITYNAMECommand(ENTITYNAMEEntity body)
        {
            Body = body;
        }
    }
}
