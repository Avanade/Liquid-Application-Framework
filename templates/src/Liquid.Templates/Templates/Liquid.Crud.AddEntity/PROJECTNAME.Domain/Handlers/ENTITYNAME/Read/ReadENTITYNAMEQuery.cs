using MediatR;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class ReadENTITYNAMEQuery : IRequest<ReadENTITYNAMEQueryResponse>
    {
        public ENTITYIDTYPE Id { get; set; }

        public ReadENTITYNAMEQuery(ENTITYIDTYPE id)
        {
            Id = id;
        }
    }
}
