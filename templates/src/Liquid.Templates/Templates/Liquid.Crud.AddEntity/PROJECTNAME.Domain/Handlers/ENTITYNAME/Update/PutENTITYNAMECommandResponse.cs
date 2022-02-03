using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class PutENTITYNAMECommandResponse
    {
        public Entities.ENTITYNAME Data { get; set; }

        public PutENTITYNAMECommandResponse(Entities.ENTITYNAME data)
        {
            Data = data;
        }
    }
}
