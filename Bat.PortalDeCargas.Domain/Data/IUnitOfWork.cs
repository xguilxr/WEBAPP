using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Data
{
    public interface IUnitOfWork
    {
        IDimensionRepository DimensionRepository { get; }
        ITemplateRepository TemplateRepository { get; }
        IUsersRepository UsersRepository { get; }

        // Task<Dimension> AddDimension(Dimension Dimension);




    }
}
