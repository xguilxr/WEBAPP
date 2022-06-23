using Bat.PortalDeCargas.Domain.Entities;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class PaginatedTemplateDTO : Template
    {
        public int TotalOfItems { get; set; }
    }
}
