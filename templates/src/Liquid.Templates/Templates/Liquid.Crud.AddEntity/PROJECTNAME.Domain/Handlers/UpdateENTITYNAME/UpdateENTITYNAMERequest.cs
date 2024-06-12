using MediatR;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class UpdateENTITYNAMERequest : IRequest<UpdateENTITYNAMEResponse>
    {
        public ENTITYNAME Body { get; set; }

        public UpdateENTITYNAMERequest(ENTITYNAME body)
        {
            Body = body;
        }
    }
}
