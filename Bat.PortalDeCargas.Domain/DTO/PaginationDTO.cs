using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class PaginationDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalOfItems { get; set; }
    }
}
