using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class DimensionDomainDTO
    {
        public int DimensionDomainId { get; set; }
        public string DomainValue { get; set; }
        public int DimensionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
