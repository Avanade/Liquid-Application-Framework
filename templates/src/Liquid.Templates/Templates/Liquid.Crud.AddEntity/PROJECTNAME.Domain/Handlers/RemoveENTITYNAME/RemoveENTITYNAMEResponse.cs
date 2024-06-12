using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class RemoveENTITYNAMEResponse
    {
        public ENTITYNAME Data { get; set; }

        public RemoveENTITYNAMEResponse(ENTITYNAME data)
        {
            Data = data;
        }
    }
}
