using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Bat.PortalDeCargas.Domain.Enums;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class Template
    {
        public Template()
        {
            Dimensions = new List<TemplateDimension>();
            Deleted = new List<int>();
        }

        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public IEnumerable<int> Deleted { get; set; }
        public IList<TemplateDimension> Dimensions { get; set; }
        public string TemplateBlobUrl { get; set; }
        public string TemplateDescription { get; set; }
        public int TemplateEndUploadWindow { get; set; }

        [JsonConverter(typeof(FileType.FileTypeEnumerationConverter))]
        public FileType TemplateFileFormat { get; set; }

        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateNotificationEmail { get; set; }
        public string TemplateNotificationText { get; set; }
        public int TemplatePeriodicity { get; set; }
        public string TemplateUpdateFeatures { get; set; }
        public string TemplateValidation { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserId { get; set; }
        public string UpdateUserName { get; set; }
    }
}
