using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplatePeriodicityFillSpecification : SyncSpecification<TemplateFormDTO>
    {
        public override bool IsSatisfiedBy(TemplateFormDTO entity) => entity.Periodicity >= 1 && entity.FileFormat <= 5;
    }
}