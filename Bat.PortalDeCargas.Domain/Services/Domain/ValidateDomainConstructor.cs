using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public class ValidateDomainConstructor: IValidateDomainConstructor
    {
               
        public ValidateDomain CreateValidator (DimensionType type, IStringLocalizer<DimensionTranslation> stringLocalizer)
        {

            switch (type)
            { 
                case DimensionType.Numero : return new ValidateNumberDomain(stringLocalizer);

                case DimensionType.Data : return new ValidateDateDomain(stringLocalizer);

                case DimensionType.Texto:  return new ValidateTextDomain(stringLocalizer);

                default: return new ValidateTextDomain(stringLocalizer);
            }

        }

    }
}
