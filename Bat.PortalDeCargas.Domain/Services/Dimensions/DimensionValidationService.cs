using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Specifications.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Domain.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Bat.PortalDeCargas.Resource.Translation;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public class DimensionValidationService : ValidationService<DimensionFormDTO, DimensionTranslation>
    {
       
        public DimensionValidationService(IUnitOfWork unitOfWork, IStringLocalizer<DimensionTranslation> stringLocalizer) : base(unitOfWork, stringLocalizer)
        {
           
        }
             

        protected override void SetValidations()
        {
            
            AddSpecification(new DimensionNameFillSpecification(), stringLocalizer["DimensionNameMustBeFilled"].Value);
            AddSpecification(new DimensionDescriptionFillSpecification(), stringLocalizer["DimensionDescriptionMustBeFilled"].Value);
            AddSpecification(new DimensionFormatFillSpecification(), stringLocalizer["DimensionFormatMustBeFille"].Value);
            AddSpecification(new DimensionStartEndNumberSpecification(), stringLocalizer["DimensionStartEndNumber"].Value);
            AddSpecification(new DimensionDupliacteNameSpecification(unitOfWork), stringLocalizer["DimensionNameDuplicated"].Value);
        }
    }
}
