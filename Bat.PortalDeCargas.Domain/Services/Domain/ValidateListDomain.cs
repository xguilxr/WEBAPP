using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public class ValidateListDomain
    {
        private IList<string> DomainList;
        public ValidateListDomain(IList<string> domainList)
        {
            this.DomainList = domainList;
        }


        public bool IsValueNotInDomain(string value)
        {
            return !this.DomainList.Any(d => d == value);

        }
    }
}
