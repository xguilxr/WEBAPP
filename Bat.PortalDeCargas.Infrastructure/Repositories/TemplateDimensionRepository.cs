using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Repositories;

namespace Bat.PortalDeCargas.Infrastructure.Repositories
{
    public class TemplateDimensionRepository : Repository<TemplateDimension>, ITemplateDimensionRepository
    {
        internal TemplateDimensionRepository(PortalDeCargasContext dbContext) : base(dbContext)
        { }

        public Task<TemplateDimension> Add(TemplateDimensionFormDTO templateDimension, IDbTransaction tran = null) =>
            Add(new
            {
                templateDimension.DimensionId,
                TemplateDimensionName = templateDimension.Name,
                TemplateDimensionOrder = templateDimension.Order,
                templateDimension.TemplateId
            }, tran);

        public Task<int> DeleteTemplateDimension
            (int? templateDimensionId, int? templateId = null, IDbTransaction tran = null) =>
            Remove(new
            {
                templateDimensionId,
                templateId
            }, tran);

        public Task<TemplateDimension> GetById(int templateDimensionId, DbTransaction tran)
        {
            object parameters = new
            {
                templateId = (int?)null,
                templateDimensionId
            };

            return FirstOrDefault<TemplateDimension>("up_GetTemplateDimension", parameters, tran);
        }

        public Task<IEnumerable<TemplateDimension>> GetByTemplate(int templateId)
        {
            object parameters = new
            {
                templateId,
                templateDimensionId = (int?)null
            };

            return Query<TemplateDimension>("up_GetTemplateDimension", parameters);
        }


        public Task<TemplateDimension> Update(TemplateDimensionFormDTO templateDimension, IDbTransaction tran = null) =>
            Update<TemplateDimension>(new
            {
                templateDimension.DimensionId,
                TemplateDimensionId = templateDimension.Id,
                TemplateDimensionName = templateDimension.Name,
                TemplateDimensionOrder = templateDimension.Order
            }, tran);
    }
}
