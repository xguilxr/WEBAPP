using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.Dimensions
{
    public class DimensionStartEndNumberSpecification : SyncSpecification<DimensionFormDTO>
    {
        public override bool IsSatisfiedBy(DimensionFormDTO entity)
        {
            return entity.StartNumber.HasValue && entity.EndNumber.HasValue ? entity.EndNumber >= entity.StartNumber: true;
        }
    }
}
