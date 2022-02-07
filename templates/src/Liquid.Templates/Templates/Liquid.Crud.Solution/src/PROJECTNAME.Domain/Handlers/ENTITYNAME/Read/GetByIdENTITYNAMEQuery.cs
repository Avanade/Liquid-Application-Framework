using MediatR;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class GetByIdENTITYNAMEQuery : IRequest<GetByIdENTITYNAMEQueryResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public GetByIdENTITYNAMEQuery(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
