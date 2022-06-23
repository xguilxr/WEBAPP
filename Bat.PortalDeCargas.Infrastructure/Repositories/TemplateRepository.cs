using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Domain.Repositories;
using Bat.PortalDeCargas.Domain.Services.Template;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bat.PortalDeCargas.Infrastructure.Repositories
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        private readonly ITemplateDimensionRepository _templateDimensionRepository;

        internal TemplateRepository
            (PortalDeCargasContext dbContext, ITemplateDimensionRepository templateDimensionRepository) : base(
            dbContext)
        {
            _templateDimensionRepository = templateDimensionRepository;
        }

        public async Task<Template> Add(TemplateFormDTO template)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await Add(new
                {
                    TemplateName = template.Name,
                    TemplateFileFormat = template.FileFormat,
                    TemplateBlobUrl = template.BlobUrl,
                    TemplatePeriodicity = template.Periodicity,
                    TemplateEndUploadWindow = template.EndUploadWindow,
                    TemplateValidation = template.Validation,
                    TemplateUpdateFeatures = template.UpdateFeatures,
                    TemplateNotificationEmail = template.NotificationEmail,
                    TemplateNotificationText = template.NotificationText,
                    TemplateDescription = template.Description,
                    template.UserId
                }, tran.GetDbTransaction());

                await Execute("up_InsertTemplateLog", new
                {
                    template.UserId,
                    result.TemplateId,
                    action = LogAction.Create
                }, tran.GetDbTransaction());

                foreach (var templateDimension in template.Dimensions)
                {
                    templateDimension.TemplateId = result.TemplateId;
                    result.Dimensions.Add(
                        await _templateDimensionRepository.Add(templateDimension, tran.GetDbTransaction()));
                }

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public async Task<int> Delete(int templateId, int userId)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                await Execute("up_InsertTemplateLog", new
                {
                    userId,
                    templateId,
                    action = LogAction.Delete
                }, tran.GetDbTransaction());

                await _templateDimensionRepository.DeleteTemplateDimension(null, templateId, tran.GetDbTransaction());
                var result = await Remove(new
                {
                    templateId
                }, tran.GetDbTransaction());

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public Task<IEnumerable<PaginatedTemplateDTO>> GetAll(string name, int page, int pageCount)
        {
            object parameters = new
            {
                TemplateName = name,
                CurrentPage = page,
                ItemsPerPage = pageCount
            };

            return Query<PaginatedTemplateDTO>("up_GetAllTemplates", parameters);
        }

        public async Task<Template> GetById(int templateId)
        {
            object parameters = new
            {
                templateId,
                templateName = (string)null
            };

            var template = await FirstOrDefault<Template>("up_GetTemplate", parameters);
            template.Dimensions = (await _templateDimensionRepository.GetByTemplate(templateId)).ToList();

            return template;
        }

        public Task<IEnumerable<Template>> GetByName(string templateName)
        {
            object parameters = new
            {
                templateName,
                templateId = (int?)null
            };

            return Query<Template>("up_GetTemplate", parameters);
        }

        public async Task<UploadLog> GetLastUpload(int templateId)
        {
            var uploadData = await SingleOrDefault<UploadLog>("up_GetLastUpload", new
            {
                templateId
            });

            var details = await Query<FileDetail>("up_GetFileDetailPaged", new
            {
                uploadData.UploadLogId,
                ShowOnlyErrors = true,
                Page = 1,
                PageSize = 20
            });

            return uploadData;
        }

        public async Task<IEnumerable<TemplateLog>> GetTemplateLog
            (TemplateLogFilter filter) =>
            await Query<TemplateLog>("up_GetTemplateLog", filter);

        public async Task<IEnumerable<FileDetailDTO>> GetUploadDetail(FileDetailFilter filter)
        {
            return await Query<FileDetailDTO>("up_GetFileDetailPaged", new
            {
                filter.UploadLogId,
                ShowOnlyErrors = false,
                filter.Page,
                filter.PageSize
            });
        }

        public async Task<IEnumerable<UploadLog>> GetUploadLog
            (UploadLogFilter filter) =>
            await Query<UploadLog>("up_GetUploadLog", filter);

        public async Task SaveValidationResult(TemplateValidationResult result, int userId)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                var p = new DynamicParameters(new
                {
                    result.TemplateId,
                    UserId = userId,
                    Status = result!.TemplateErrors.Any(),
                    TotalLines = result.Rows.Count(),
                    TotalInvalidLines = result.Rows.Count(r => r.CellValidationResults.Any()),
                    result.FileName,
                    TotalValidationMessages = result.TemplateErrors.Count()
                });

                p.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await Execute("up_InsertUploadLog", p, tran.GetDbTransaction());
                var uploadLogId = p.Get<int>("@result");

                foreach (var i in result.Rows.SelectMany(row => row.CellValidationResults.Select(cell => new
                         {
                             row,
                             cell
                         })))
                {
                    var r = i.row;
                    var c = i.cell;
                    await Execute("up_InsertFileDetail", new
                    {
                        UploadLogId = uploadLogId,
                        r.RowNumber,
                        TemplateDimensionID = c.TemplateDimensionId,
                        c.Value,
                        ValidationMessage = string.Join("\n", c.Errors)
                    }, tran.GetDbTransaction());
                }

                await tran.CommitAsync();
                result.UploadLogId = uploadLogId;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public async Task<Template> Update(TemplateFormDTO template)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                await Execute("up_InsertTemplateLog", new
                {
                    template.UserId,
                    TemplateId = template.Id,
                    action = LogAction.Update
                }, tran.GetDbTransaction());

                var result = await Update<Template>(new
                {
                    TemplateId = template.Id,
                    TemplateName = template.Name,
                    TemplateFileFormat = template.FileFormat,
                    TemplateBlobUrl = template.BlobUrl,
                    TemplatePeriodicity = template.Periodicity,
                    TemplateEndUploadWindow = template.EndUploadWindow,
                    TemplateValidation = template.Validation,
                    TemplateUpdateFeatures = template.UpdateFeatures,
                    TemplateNotificationEmail = template.NotificationEmail,
                    TemplateNotificationText = template.NotificationText,
                    TemplateDescription = template.Description,
                    template.UserId
                }, tran.GetDbTransaction());

                foreach (var deleted in template.Deleted)
                {
                    await _templateDimensionRepository.DeleteTemplateDimension(deleted, tran: tran.GetDbTransaction());
                }

                foreach (var templateDimension in template.Dimensions)
                {
                    if (templateDimension.IsUpdated)
                    {
                        result.Dimensions.Add(
                            await _templateDimensionRepository.Update(templateDimension, tran.GetDbTransaction()));
                    }
                    else if (templateDimension.Id == 0)
                    {
                        result.Dimensions.Add(
                            await _templateDimensionRepository.Add(templateDimension, tran.GetDbTransaction()));
                    }
                    else
                    {
                        result.Dimensions.Add(
                            await _templateDimensionRepository.GetById(templateDimension.Id, tran.GetDbTransaction()));
                    }
                }

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }
    }
}
