using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class ReadENTITYNAMEQueryResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public ReadENTITYNAMEQueryResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
