using System;
using Bat.PortalDeCargas.Domain.Enums;
using Newtonsoft.Json;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class DimensionLog
    {
        [JsonConverter(typeof(LogAction.LogActionEnumerationConverter))]
        public LogAction Action { get; set; }
        public DateTime ActionDate { get; set; }
        public int DimensionId { get; set; }
        public int DimensionLogId { get; set; }
        public string DimensionName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalOfItems { get; set; }
    }
}