using System.Linq;
using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateDimensionOrder : SyncSpecification<TemplateFormDTO>
    {
        public override bool IsSatisfiedBy(TemplateFormDTO entity)
        {
            // A obrigatoriedade das dimensões é validada em outra regra
            if (!entity.Dimensions.Any())
            {
                return true;
            }

            var ordered = entity.Dimensions.OrderBy(e => e.Order).ToList();

            if (ordered[0].Order != 1)
            {
                return false;
            }

            var idx = 1;

            for (var i = 1; i < ordered.Count(); i++)
            {
                if (ordered[i].Order != ++idx)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
