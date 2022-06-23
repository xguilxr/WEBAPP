using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateFileFormatFillSpecification : SyncSpecification<TemplateFormDTO>
    {
        public override bool IsSatisfiedBy(TemplateFormDTO entity) => entity.FileFormat >= 1 && entity.FileFormat <= 3;
    }
}
