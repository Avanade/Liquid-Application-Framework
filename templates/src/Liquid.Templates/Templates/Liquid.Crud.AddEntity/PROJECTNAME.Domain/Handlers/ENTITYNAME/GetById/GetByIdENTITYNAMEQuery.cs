using MediatR;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.GetById
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
