using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateValidationService : ValidationService<TemplateFormDTO, TemplateTranslation>
    {
        public TemplateValidationService
            (IUnitOfWork unitOfWork, IStringLocalizer<TemplateTranslation> stringLocalizer) : base(unitOfWork,
            stringLocalizer)
        { }

        protected override void SetValidations()
        {
            AddSpecification(new TemplateNameFillSpecification(), stringLocalizer["TemplateNameMustBeFilled"].Value);
            AddSpecification(new TemplateNameUnicitySpecification(unitOfWork),
                stringLocalizer["TemplateNameMustBeUnique"].Value);

            AddSpecification(new TemplateFileFormatFillSpecification(),
                stringLocalizer["TemplateFileFormatMustBeFilled"].Value);

            AddSpecification(new TemplateBlobUrlFillSpecification(),
                stringLocalizer["TemplateBlobUrlMustBeValidUrl"].Value);

            AddSpecification(new TemplateDescriptionFillSpecification(),
                stringLocalizer["TemplateDescriptionMustBeFilled"].Value);

            AddSpecification(new TemplateEndUploadWindowFillSpecification(),
                stringLocalizer["TemplateEndUploadWindowInvalid"].Value);

            AddSpecification(new TemplateNotificationEmailFillSpecification(),
                stringLocalizer["TemplateNotificationEmailInvalid"].Value);

            AddSpecification(new TemplateNotificationTextFillSpecification(),
                stringLocalizer["TemplateNotificationTextMustBeFilled"].Value);

            AddSpecification(new TemplatePeriodicityFillSpecification(),
                stringLocalizer["TemplatePeriodicityInvalid"].Value);

            AddSpecification(new TemplateDimensionOrder(), stringLocalizer["TemplateDimensionOrderGap"].Value);
        }
    }
}
