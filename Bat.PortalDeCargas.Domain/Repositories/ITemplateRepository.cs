using System.Collections.Generic;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Services.Template;

namespace Bat.PortalDeCargas.Domain.Repositories
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<Template> Add(TemplateFormDTO template);
        Task<int> Delete(int templateId, int userId);
        Task<IEnumerable<PaginatedTemplateDTO>> GetAll(string name, int page, int pageCount);
        Task<Template> GetById(int templateId);
        Task<IEnumerable<Template>> GetByName(string entityName);
        Task<UploadLog> GetLastUpload(int templateId);
        Task SaveValidationResult(TemplateValidationResult result, int userId);
        Task<Template> Update(TemplateFormDTO template);
        Task<IEnumerable<TemplateLog>> GetTemplateLog(TemplateLogFilter filter);
        Task<IEnumerable<UploadLog>> GetUploadLog(UploadLogFilter filter);
        Task<IEnumerable<FileDetailDTO>> GetUploadDetail(FileDetailFilter filter);
    }
}
