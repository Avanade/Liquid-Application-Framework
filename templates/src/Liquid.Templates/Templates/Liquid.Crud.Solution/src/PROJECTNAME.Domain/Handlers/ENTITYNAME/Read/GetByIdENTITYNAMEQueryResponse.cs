using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class GetByIdENTITYNAMEQueryResponse
    {
        public ENTITYNAMEEntity Data { get; set; }

        public GetByIdENTITYNAMEQueryResponse(ENTITYNAMEEntity data)
        {
            Data = data;
        }
    }
}
