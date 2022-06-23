using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class DimensionDomain
    {
        public int DimensionDomainId { get; set; }
        public string DomainValue { get; set; }
        public int DimensionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
