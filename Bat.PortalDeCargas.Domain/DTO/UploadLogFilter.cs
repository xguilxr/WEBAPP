using System;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class PagerFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class UploadLogFilter : PagerFilter
    {
        public string TemplateName { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string UserName { get; set; }
    }

    public class FileDetailFilter:PagerFilter
    {
        public int UploadLogId { get; set; }
    }
}
