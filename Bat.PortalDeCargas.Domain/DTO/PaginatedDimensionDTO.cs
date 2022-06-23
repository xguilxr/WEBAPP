using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class PaginatedDimensionDTO:DimensionDTO
    {
        public int TotalOfItems { get; set; }
    }
}
