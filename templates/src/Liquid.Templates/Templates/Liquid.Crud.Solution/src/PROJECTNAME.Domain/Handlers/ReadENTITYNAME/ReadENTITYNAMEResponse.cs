using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers
{
    public class ReadENTITYNAMEResponse
    {
        public ENTITYNAME Data { get; set; }

        public ReadENTITYNAMEResponse(ENTITYNAME data)
        {
            Data = data;
        }
    }
}
