using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Repositories;
using Bat.PortalDeCargas.Domain.Services.Azure;
using Bat.PortalDeCargas.Domain.Services.Domain;
using Bat.PortalDeCargas.Resource.Translation;
using Impeto.Framework.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateService : ITemplateService
    {
        private readonly IStringLocalizer<DimensionTranslation> _dimensionStringLocalizer;
        private readonly IAzureIntegrationService _azureIntegrationService;
        private readonly IStringLocalizer<TemplateTranslation> _stringLocalizer;
        private readonly IUnitOfWork _unitOfWork;

        public TemplateService
        (IUnitOfWork unitOfWork, IStringLocalizer<TemplateTranslation> stringLocalizer,
            IStringLocalizer<DimensionTranslation> dimensionStringLocalizer, IAzureIntegrationService azureIntegrationService)
        {
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;
            _dimensionStringLocalizer = dimensionStringLocalizer;
            _azureIntegrationService = azureIntegrationService;
        }

        public async Task<ValidationResult> Delete(int templateId, int userId)
        {
            var result = await _unitOfWork.TemplateRepository.Delete(templateId, userId) > 0;

            return result ? new ValidationResult() : new ValidationResult(_stringLocalizer["TemplateNotFound"]);
        }

        public async Task<(MemoryStream, string, string)> GenerateFile(int templateId)
        {
            var template = await _unitOfWork.TemplateRepository.GetById(templateId);
            var fileGenerator = template.TemplateFileFormat.FileGenerator;

            return (fileGenerator.Generate(template.Dimensions.ToList()),
                $"{template.TemplateName}{(string)template.TemplateFileFormat}",
                template.TemplateFileFormat.ContentType);
        }

        public async Task<PaginationDTO<PaginatedTemplateDTO>> GetAll(string name, int page, int pageCount)
        {
            var templates = (await _unitOfWork.TemplateRepository.GetAll(name, page, pageCount)).ToList();
            var amount = 0;

            if (templates.Any())
            {
                amount = templates.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<PaginatedTemplateDTO>
            {
                Items = templates,
                CurrentPage = page,
                ItemsPerPage = pageCount,
                TotalOfItems = amount
            };
        }

        public async Task<UploadLog> GetLastUpload
            (int templateId) =>
            await _unitOfWork.TemplateRepository.GetLastUpload(templateId);

        public async Task<Entities.Template> GetTemplateById(int templateId)
        {
            var template = await _unitOfWork.TemplateRepository.GetById(templateId);

            return template;
        }

        public async Task<PaginationDTO<TemplateLog>> GetTemplateLog(TemplateLogFilter filter)
        {
            var log = (await _unitOfWork.TemplateRepository.GetTemplateLog(filter)).ToList();
            var amount = 0;

            if (log.Any())
            {
                amount = log.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<TemplateLog>
            {
                Items = log,
                CurrentPage = filter.Page,
                ItemsPerPage = filter.PageSize,
                TotalOfItems = amount
            };
        }

        public async Task<PaginationDTO<UploadLog>> GetUploadLog(UploadLogFilter filter)
        {
            var log = (await _unitOfWork.TemplateRepository.GetUploadLog(filter)).ToList();
            var amount = 0;

            if (log.Any())
            {
                amount = log.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<UploadLog>
            {
                Items = log,
                CurrentPage = filter.Page,
                ItemsPerPage = filter.PageSize,
                TotalOfItems = amount
            };
        }

        public async Task<PaginationDTO<FileDetailDTO>> GetUploadDetail(FileDetailFilter filter)
        {
            IEnumerable<FileDetailDTO> log = (await _unitOfWork.TemplateRepository.GetUploadDetail(filter)).ToList();
            var amount = 0;

            if (log.Any())
            {
                amount = log.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<FileDetailDTO>
            {
                Items = log,
                CurrentPage = filter.Page,
                ItemsPerPage = filter.PageSize,
                TotalOfItems = amount
            };
        }

        public async Task<ResultDTO<Entities.Template>> Save(TemplateFormDTO template)
        {
            var validationResult =
                await new TemplateValidationService(_unitOfWork, _stringLocalizer).Validate(template);

            Entities.Template result = null;

            if (validationResult.IsValid)
            {
                result = template.Id == 0
                    ? await _unitOfWork.TemplateRepository.Add(template)
                    : await _unitOfWork.TemplateRepository.Update(template);
            }

            return new ResultDTO<Entities.Template>
            {
                Erros = validationResult.IsValid ? null : validationResult.Errors.Distinct(),
                IndSucesso = validationResult.IsValid,
                Model = result
            };
        }

        public async Task<TemplateValidationResult> UploadFile(IFormFile file, int templateId, int userId)
        {
            var template = await _unitOfWork.TemplateRepository.GetById(templateId);
            var result = new TemplateValidationResult(templateId,
                $"{template.TemplateName}_resultado{template.TemplateFileFormat.Name}");

            if (!string.Equals(file.ContentType, template.TemplateFileFormat.ContentType, StringComparison.Ordinal))
            {
                result.AddTemplateError(_stringLocalizer["InvalidFileFormat"]);

                return result;
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets["Result"];
            var rowCount = worksheet.Dimension.End.Row;
            var dimensionStore = new DimensionStore(_unitOfWork.DimensionRepository, _dimensionStringLocalizer);

            if (rowCount < 2)
            {
                result.AddTemplateError(_stringLocalizer["EmptyFile"]);
            }

            for (var r = 2; r <= rowCount; r++)
            {
                await ValidateRow(r, dimensionStore,
                    template.Dimensions.OrderBy(d => d.TemplateDimensionOrder).ToList(), worksheet, result);
            }

            foreach (var rowValidationError in result.Rows)
            {
                worksheet.Cells[rowValidationError.RowNumber, template.Dimensions.Count + 1].Value =
                    rowValidationError.GetErrors();
            }

            worksheet.Columns[template.Dimensions.Count + 1].AutoFit();
            using var resultStream = new MemoryStream();
            await package.SaveAsAsync(resultStream);
            result.Stream = resultStream.ToArray();
            await _unitOfWork.TemplateRepository.SaveValidationResult(result, userId);

            if (result.IsValid)
            {
                await _azureIntegrationService.PostBlobAsync(result.Stream, template);
            }

            return result;
        }

        private static async Task<TemplateValidationCellResult> ValidateCell
            (ExcelRangeBase cell, TemplateDimension dimension, DimensionStore dimensionStore, string value)
        {
            var validatioResult = (await dimensionStore.GetValidator(dimension.DimensionId)).IsValidDomain(
                await dimensionStore.GetDimension(dimension.DimensionId), cell.GetValue<string>());

            return new TemplateValidationCellResult
            {
                TemplateDimensionId = dimension.TemplateDimensionId,
                Dimension = await dimensionStore.GetDimension(dimension.DimensionId),
                ColumnName = dimension.TemplateDimensionName,
                ColumnOrder = dimension.TemplateDimensionOrder,
                Errors = validatioResult,
                Value = value
            };
        }

        private static async Task ValidateRow
        (int rowNumber, DimensionStore dimensionStore, IList<TemplateDimension> dimensions, ExcelWorksheet sheet,
            TemplateValidationResult result)
        {
            var templateRowValidationError = new TemplateRowValidationResult(rowNumber);
            result.AddRowValidationError(templateRowValidationError);

            for (var c = 1; c <= dimensions.Count; c++)
            {
                var cell = sheet.Cells[rowNumber, c];
                var dimension = dimensions[c - 1];
                var cellValidationResult = await ValidateCell(cell, dimension, dimensionStore, cell.GetValue<string>());
                templateRowValidationError.CellValidationResults.Add(cellValidationResult);
            }
        }

        private class DimensionStore
        {
            private readonly Dictionary<int, DimensionDTO> _dimensions;
            private readonly IDimensionRepository _repository;
            private readonly IStringLocalizer<DimensionTranslation> _stringLocalizer;
            private readonly Dictionary<int, ValidateDomain> _validators;

            public DimensionStore
                (IDimensionRepository repository, IStringLocalizer<DimensionTranslation> stringLocalizer)
            {
                _repository = repository;
                _stringLocalizer = stringLocalizer;
                _dimensions = new Dictionary<int, DimensionDTO>();
                _validators = new Dictionary<int, ValidateDomain>();
            }

            public async Task<DimensionDTO> GetDimension(int dimensionId)
            {
                if (!_dimensions.ContainsKey(dimensionId))
                {
                    var dimension = await _repository.GetDimensionById(dimensionId);
                    dimension.Domains = await _repository.GetDimensionDomainById(dimensionId);
                    _dimensions.Add(dimensionId, dimension);
                }

                return _dimensions[dimensionId];
            }

            public async Task<ValidateDomain> GetValidator(int dimensionId)
            {
                if (!_validators.ContainsKey(dimensionId))
                {
                    _validators.Add(dimensionId,
                        new ValidateDomainConstructor().CreateValidator((await GetDimension(dimensionId)).DimensionType,
                            _stringLocalizer));
                }

                return _validators[dimensionId];
            }
        }
    }
}
