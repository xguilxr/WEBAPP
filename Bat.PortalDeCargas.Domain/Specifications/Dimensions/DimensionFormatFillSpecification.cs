using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Enums;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.Dimensions
{
    public class DimensionFormatFillSpecification : SyncSpecification<DimensionFormDTO>
    {
        public override bool IsSatisfiedBy(DimensionFormDTO entity)
        {
            return entity.Type == DimensionType.Data ? !string.IsNullOrEmpty(entity.Format) : true;
        }
    }
}
