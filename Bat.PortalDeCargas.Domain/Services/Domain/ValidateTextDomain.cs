using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public class ValidateTextDomain:ValidateDomain
    {

        public ValidateTextDomain(IStringLocalizer<DimensionTranslation> stringLocalizer) : base(stringLocalizer)
        {

        }
        public override IList<string> IsValidDomain(DimensionDTO Dimension, string value)
        {
            var erros = new List<string>();

            if (Dimension.DimensionSize> 0)
                if (value.Length > Dimension.DimensionSize)
                    erros.Add(string.Format(this.stringLocalizer["DimensionInvalidLengh"].Value, Dimension.DimensionSize));

            return erros;
        }
    }
}
