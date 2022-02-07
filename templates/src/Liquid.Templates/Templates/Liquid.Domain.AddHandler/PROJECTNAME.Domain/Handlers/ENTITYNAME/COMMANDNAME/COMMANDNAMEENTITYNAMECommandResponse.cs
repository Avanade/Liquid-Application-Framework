using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME
{
    public class COMMANDNAMEENTITYNAMECommandResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public COMMANDNAMEENTITYNAMECommandResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
