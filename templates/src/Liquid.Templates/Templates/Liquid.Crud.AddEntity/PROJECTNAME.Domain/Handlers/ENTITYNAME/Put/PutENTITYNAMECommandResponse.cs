using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Put
{
    public class PutENTITYNAMECommandResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public PutENTITYNAMECommandResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
