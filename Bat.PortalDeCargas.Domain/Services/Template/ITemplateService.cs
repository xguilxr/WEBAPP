using System.IO;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Impeto.Framework.Domain.Service;
using Microsoft.AspNetCore.Http;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public interface ITemplateService
    {
        Task<ValidationResult> Delete(int templateId, int userId);
        Task<(MemoryStream, string, string)> GenerateFile(int templateId);
        Task<PaginationDTO<PaginatedTemplateDTO>> GetAll(string name, int page, int pageCount);
        Task<UploadLog> GetLastUpload(int templateId);
        Task<Entities.Template> GetTemplateById(int templateId);
        Task<ResultDTO<Entities.Template>> Save(TemplateFormDTO template);
        Task<TemplateValidationResult> UploadFile(IFormFile file, int templateId, int userId);
        Task<PaginationDTO<TemplateLog>> GetTemplateLog(TemplateLogFilter filter);
        Task<PaginationDTO<UploadLog>> GetUploadLog(UploadLogFilter filter);
        Task<PaginationDTO<FileDetailDTO>> GetUploadDetail(FileDetailFilter filter);
    }
}
