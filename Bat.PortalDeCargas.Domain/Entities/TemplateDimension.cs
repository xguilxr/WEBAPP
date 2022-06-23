using System;
using Bat.PortalDeCargas.Domain.DTO;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class TemplateDimension
    {
        public int TemplateDimensionId { get; set; }
        public int TemplateId { get; set; }
        public int DimensionId { get; set; }
        public string DimensionName { get; set; }
        public int TemplateDimensionOrder { get; set; }
        public string TemplateDimensionName { get; set; }
        public bool IsUpdated { get; set; }

    }
}
