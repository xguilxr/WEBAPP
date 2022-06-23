using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Domain
{
    public class ValidateNumberDomain:ValidateDomain
    {
        public ValidateNumberDomain(IStringLocalizer<DimensionTranslation> stringLocalizer) : base(stringLocalizer)
        {

        }

        public override IList<string> IsValidDomain(DimensionDTO Dimension, string value)
        {
            var erros = new List<string>();
           
            var number = 0;

            if( !int.TryParse(value, out number) )
            {
                erros.Add(this.stringLocalizer["DimensioInvalidNumber"].Value);
                return erros;
            }
                
             if (number < Dimension.DimensionStartNumber)
                erros.Add(this.stringLocalizer["DimensionInvalidStartNumber"].Value);


            if (number > Dimension.DimensionEndNumber)
                erros.Add(this.stringLocalizer["DimensionInvalidEndNumber"].Value);


            return erros;
        }
    }
}
