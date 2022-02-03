using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete
{
    public class DeleteENTITYNAMECommandResponse
    {
        public Entities.ENTITYNAME Data { get; set; }

        public DeleteENTITYNAMECommandResponse(Entities.ENTITYNAME data)
        {
            Data = data;
        }
    }
}
