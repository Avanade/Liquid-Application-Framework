using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class UpdateENTITYNAMEResponse
    {
        public ENTITYNAME Data { get; set; }

        public UpdateENTITYNAMEResponse(ENTITYNAME data)
        {
            Data = data;
        }
    }
}
