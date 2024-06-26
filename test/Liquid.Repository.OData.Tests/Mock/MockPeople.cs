using Liquid.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.OData.Tests.Mock
{
    public class People : LiquidEntity<string>
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Emails { get; set; }

        public string AddressInfo { get; set; }

        public string Gender { get; set; }

        public string Concurrency { get; set; }

    }
}
