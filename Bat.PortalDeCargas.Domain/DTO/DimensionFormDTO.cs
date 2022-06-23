using Bat.PortalDeCargas.Domain.Enums;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class DimensionFormDTO
    {
        public string Description { get; set; }
        public int? EndNumber { get; set; }
        public string Format { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Size { get; set; }
        public int? StartNumber { get; set; }
        public DimensionType Type { get; set; }
        public int UserId { get; set; }
    }
}
