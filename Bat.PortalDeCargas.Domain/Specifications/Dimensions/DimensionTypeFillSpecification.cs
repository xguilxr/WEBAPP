using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.Dimensions
{
    public class DimensionTypeFillSpecification : SyncSpecification<DimensionFormDTO>
    {
        public override bool IsSatisfiedBy(DimensionFormDTO entity)
        {
            var type = Convert.ToInt32(entity.Type);
            return type >0 && type <= 3 ;
        }
    }
}
