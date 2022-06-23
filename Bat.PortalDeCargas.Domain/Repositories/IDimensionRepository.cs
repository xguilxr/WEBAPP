using System.Collections.Generic;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;

namespace Bat.PortalDeCargas.Domain.Repositories
{
    public interface IDimensionRepository : IRepository<Dimension>
    {
        Task<Dimension> AddDimension(DimensionFormDTO dimension);
        Task<int> AddDimensionDomain(IEnumerable<DimensionDomainDTO> DimensionsDomain);
        Task<int> Copy(int dimensionId, string dimensionName);
        Task<int> Delete(int dimensionId, int userId);
        Task<int> DeleteDimensionDomain(int idDimension);
        Task<IEnumerable<PaginatedDimensionDTO>> GetAllDimension(string name, int page, int pageCount);
        Task<IEnumerable<DimensionFilterDTO>> GetDimensionByFilter(string name);
        Task<DimensionDTO> GetDimensionById(int id);
        Task<IEnumerable<DimensionDTO>> GetDimensionByName(string name);
        Task<IEnumerable<DimensionDomainDTO>> GetDimensionDomainById(int idDimension);
        Task<Dimension> UpdateDimension(DimensionFormDTO Dimension);
        Task<IEnumerable<DimensionLog>> GetDimensionLog(DimensionLogFilter filter);
    }
}
