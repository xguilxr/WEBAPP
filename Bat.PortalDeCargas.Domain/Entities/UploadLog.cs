using System;
using System.Collections.Generic;
using Bat.PortalDeCargas.Domain.DTO;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class UploadLog
    {
        public UploadLog()
        {
            Details = new PaginationDTO<FileDetailDTO>();
        }

        public PaginationDTO<FileDetailDTO> Details { get; set; }
        public string FileName { get; set; }
        public bool Status { get; set; }
        public string TemplateName { get; set; }
        public int TotalInvalidLines { get; set; }
        public int TotalLines { get; set; }
        public int TotalValidationMessages { get; set; }
        public DateTime UploadLogDate { get; set; }
        public int UploadLogId { get; set; }
        public string UserName { get; set; }
        public int TotalOfItems { get; set; }
    }
}
