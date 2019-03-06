using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class ClientContractArr
    {
        public IEnumerable<PZ_Client> ClassClient { get; set; }
        public IEnumerable<PZ_Contract> ClassContract { get; set; }
        public IEnumerable<PZ_ContractArr> ClassContractArr { get; set; }
    }
}