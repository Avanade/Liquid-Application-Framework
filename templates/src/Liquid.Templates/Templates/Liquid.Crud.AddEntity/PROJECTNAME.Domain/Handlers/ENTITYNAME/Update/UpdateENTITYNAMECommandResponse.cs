using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class UpdateENTITYNAMECommandResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public UpdateENTITYNAMECommandResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
