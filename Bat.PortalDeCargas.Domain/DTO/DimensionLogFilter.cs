using System;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class DimensionLogFilter
    {
        public string DimensionName { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public string UserName { get; set; }
    }
}
