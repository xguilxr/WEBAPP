using Bat.PortalDeCargas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class PaginatedUsersDTO:AppUser
    {
        public int TotalOfItems { get; set; }
    }
}
