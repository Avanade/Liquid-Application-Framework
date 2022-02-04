using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete
{
    public class DeleteENTITYNAMECommandResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public DeleteENTITYNAMECommandResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
