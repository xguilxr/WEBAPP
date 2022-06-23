using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bat.PortalDeCargas.Domain.Repositories
{
    public interface ITemplateDimensionRepository : IRepository<TemplateDimension>
    {
        Task<TemplateDimension> Add(TemplateDimensionFormDTO templateDimension, IDbTransaction tran = null);

        Task<int> DeleteTemplateDimension
            (int? templateDimensionId = null, int? templateId = null, IDbTransaction tran = null);

        Task<TemplateDimension> Update(TemplateDimensionFormDTO templateDimension, IDbTransaction tran = null);
        Task<IEnumerable<TemplateDimension>> GetByTemplate(int templateId);
        Task<TemplateDimension> GetById(int templateDimensionId, DbTransaction tran);
        
    }
}
