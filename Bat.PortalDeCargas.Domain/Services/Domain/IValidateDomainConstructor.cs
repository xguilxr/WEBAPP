using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public interface IValidateDomainConstructor
    {
        ValidateDomain CreateValidator(DimensionType type, IStringLocalizer<DimensionTranslation> stringLocalizer);
    }
}
