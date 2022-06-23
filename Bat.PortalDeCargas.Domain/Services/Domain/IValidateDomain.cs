using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public abstract class ValidateDomain 
    {
        protected IStringLocalizer<DimensionTranslation> stringLocalizer;

        public ValidateDomain(IStringLocalizer<DimensionTranslation> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }
        public abstract IList<string> IsValidDomain(DimensionDTO Dimension, string value);
    }
}
