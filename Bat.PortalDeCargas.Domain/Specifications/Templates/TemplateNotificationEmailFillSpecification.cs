using System.Text.RegularExpressions;
using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateNotificationEmailFillSpecification : SyncSpecification<TemplateFormDTO>
    {
        public override bool IsSatisfiedBy(TemplateFormDTO entity) =>
            !string.IsNullOrWhiteSpace(entity.NotificationEmail) && Regex.IsMatch(entity.NotificationEmail,
                @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
    }
}