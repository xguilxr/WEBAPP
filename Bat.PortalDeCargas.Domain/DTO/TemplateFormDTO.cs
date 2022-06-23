using System.Collections.Generic;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class TemplateFormDTO
    {
        public TemplateFormDTO()
        {
            Dimensions = new List<TemplateDimensionFormDTO>();
            Deleted = new List<int>();
        }

        public string BlobUrl { get; set; }
        public IEnumerable<int> Deleted { get; set; }
        public string Description { get; set; }
        public IEnumerable<TemplateDimensionFormDTO> Dimensions { get; set; }
        public int EndUploadWindow { get; set; }
        public int FileFormat { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string NotificationEmail { get; set; }
        public string NotificationText { get; set; }
        public int Periodicity { get; set; }
        public string UpdateFeatures { get; set; }
        public int UserId { get; set; }
        public string Validation { get; set; }
    }
}
