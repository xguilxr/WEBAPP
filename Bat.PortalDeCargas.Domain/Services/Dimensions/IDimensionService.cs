using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Impeto.Framework.Domain.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public interface IDimensionService
    {
        Task<ResultDTO<Dimension>> Save(DimensionFormDTO Dimension);
        Task<PaginationDTO<PaginatedDimensionDTO>> GetAll(string name, int page);
        Task<ValidationResult> Delete(int dimensionId, int userId);
        Task<ValidationResult> CopyDimension(int dimensionId, string dimensionName);
        Task<DimensionDTO> GetDimensionById(int dimensionId);
        Task<ValidationResult> DeleteDimensionDomain(int dimensionId);
        Task<MemoryStream> GenerateExcelFile(string name);
        Task<IEnumerable<DimensionDTO>> GetAllDimensions();
        Task<PaginationDTO<DimensionLog>> GetDimensionLog(DimensionLogFilter filter);
    }
}
