using System.Linq;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateNameUnicitySpecification : SyncSpecification<TemplateFormDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TemplateNameUnicitySpecification(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override bool IsSatisfiedBy
            (TemplateFormDTO entity)
        {
            var templates =_unitOfWork.TemplateRepository.GetByName(entity.Name).Result;

            return templates.All(t => entity.Id == t.TemplateId);
        }
    }
}