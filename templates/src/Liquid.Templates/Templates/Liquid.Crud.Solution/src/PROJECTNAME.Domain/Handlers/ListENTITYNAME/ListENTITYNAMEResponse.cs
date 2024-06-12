using PROJECTNAME.Domain.Entities;
using System.Collections.Generic;

namespace PROJECTNAME.Domain.Handlers
{
    public class ListENTITYNAMEResponse
    {
        public IEnumerable<ENTITYNAME> Data { get; set; }

        public ListENTITYNAMEResponse(IEnumerable<ENTITYNAME> data)
        {
            Data = data;
        }
    }
}
