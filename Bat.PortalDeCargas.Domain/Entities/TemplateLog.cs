using System;
using Bat.PortalDeCargas.Domain.Enums;
using Newtonsoft.Json;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class TemplateLog
    {
        [JsonConverter(typeof(LogAction.LogActionEnumerationConverter))]
        public LogAction Action { get; set; }
        public DateTime ActionDate { get; set; }
        public int TemplateId { get; set; }
        public int TemplateLogId { get; set; }
        public string TemplateName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalOfItems { get; set; }
    }
}
