using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateEndUploadWindowFillSpecification : SyncSpecification<TemplateFormDTO>
    {
        public override bool IsSatisfiedBy(TemplateFormDTO entity)
        {
            var valor = entity.EndUploadWindow;

            switch (entity.Periodicity)
            {
                case 1: // Mensal
                    return valor >= 1 && valor <= 31;
                case 2: // Diário
                    return true;
                case 3: // Semanal
                    return valor >= 1 && valor <= 7;
                case 4: // Anual
                case 5: // Trimenstral
                    return valor >= 1 && valor <= 12;
                default: // Periodicidade possui sua própria validação
                    return true;
            }
        }
    }
}