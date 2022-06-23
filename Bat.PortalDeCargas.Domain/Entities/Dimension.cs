using Bat.PortalDeCargas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class Dimension
    {
        public int DimensionId { get; set; }
        public string DimensionName { get; set; }
        public DimensionType DimensionType { get; set; }
        public string Type { get; set; }
        public int DimensionSize { get; set; }
        public string DimensionFormat { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedUserName { get; set; }
        public string UpdatedUserName { get; set; }
        public int? DimensionStartNumber { get; set; }
        public int? DimensionEndNumber { get; set; }
        public string DimensionDescription { get; set; }
        public IEnumerable<DimensionDomain> Domains { get; set; }



    }
}
