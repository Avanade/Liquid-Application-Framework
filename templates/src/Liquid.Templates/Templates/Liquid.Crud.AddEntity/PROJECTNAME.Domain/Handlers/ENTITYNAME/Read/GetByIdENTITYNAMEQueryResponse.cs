using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class GetByIdENTITYNAMEQueryResponse
    {
        public Entities.ENTITYNAME Data { get; set; }

        public GetByIdENTITYNAMEQueryResponse(Entities.ENTITYNAME data)
        {
            Data = data;
        }
    }
}
