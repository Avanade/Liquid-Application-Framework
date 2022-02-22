using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Remove
{
    public class RemoveENTITYNAMECommandResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public RemoveENTITYNAMECommandResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
