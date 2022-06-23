using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Specifications.Dimensions
{
    class DimensionDupliacteNameSpecification : SyncSpecification<DimensionFormDTO>
    {
        
        protected readonly IUnitOfWork unitOfWork;
     
        public DimensionDupliacteNameSpecification(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
               
        public override bool IsSatisfiedBy(DimensionFormDTO entity)
        {
            var DimensionsDTO =  this.unitOfWork.DimensionRepository.GetDimensionByName(entity.Name).Result;

            return ! DimensionsDTO.Any(d => d.DimensionName.Equals(entity.Name) && d.DimensionId != entity.Id);
        }
    }
}
