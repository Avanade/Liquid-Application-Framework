using PROJECTNAME.Domain.Entities;
using System.Collections.Generic;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.List
{
    public class ListENTITYNAMEQueryResponse
    {
        public IEnumerable<ENTITYNAMEEntity> Data { get; set; }

        public ListENTITYNAMEQueryResponse(IEnumerable<ENTITYNAMEEntity> data)
        {
            Data = data;
        }
    }
}
