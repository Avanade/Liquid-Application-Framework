using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.GetById
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
