using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.Dimensions
{
    public class DimensionDescriptionFillSpecification : SyncSpecification<DimensionFormDTO>
    {
        public override bool IsSatisfiedBy(DimensionFormDTO entity)
        {
            return !string.IsNullOrWhiteSpace(entity.Description);
        }
    }
}
