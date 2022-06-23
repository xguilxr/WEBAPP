using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Services.Files;
using Bat.PortalDeCargas.Resource.Translation;
using Impeto.Framework.Domain.Service;
using Microsoft.Extensions.Localization;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public class DimensionService : IDimensionService
    {
        private readonly IAppConfiguration _appConfig;
        private readonly IStringLocalizer<DimensionTranslation> _stringLocalizer;
        private readonly IUnitOfWork _unitOfWork;

        public DimensionService
        (IUnitOfWork unitOfWork, IAppConfiguration appConfig,
            IStringLocalizer<DimensionTranslation> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _appConfig = appConfig;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ValidationResult> CopyDimension(int dimensionId, string dimensioName)
        {
            var retorno = new ValidationResult(new List<string>());

            if (string.IsNullOrEmpty(dimensioName))
            {
                retorno.AddError(_stringLocalizer["DimensionNameMustBeFilled"].Value);
            }

            var dimension = await _unitOfWork.DimensionRepository.GetDimensionByName(dimensioName);

            if (dimension.Any())
            {
                retorno.AddError(_stringLocalizer["DimensionNameDuplicated"].Value);
            }

            if (!retorno.Errors.Any())
            {
                await _unitOfWork.DimensionRepository.Copy(dimensionId, dimensioName);
            }

            return retorno;
        }

        public async Task<ValidationResult> Delete(int dimensionId, int userId)
        {
            var result = await _unitOfWork.DimensionRepository.Delete(dimensionId, userId) > 0;

            return result ? new ValidationResult() : new ValidationResult(_stringLocalizer["DimensionNotFound"].Value);
        }

        public async Task<ValidationResult> DeleteDimensionDomain(int dimensionId)
        {
            var result = await _unitOfWork.DimensionRepository.DeleteDimensionDomain(dimensionId) > 0;

            return result ? new ValidationResult() : new ValidationResult(_stringLocalizer["DimensionNotFound"].Value);
        }

        public async Task<MemoryStream> GenerateExcelFile(string name)
        {
            var dimensions = await _unitOfWork.DimensionRepository.GetDimensionByFilter(name);
            var fileGenerator =
                new FileGeneratorServiceService<DimensionFilterDTO>(new ExcelFileGenerate<DimensionFilterDTO>());

            return fileGenerator.Generate(dimensions.ToList());
        }

        public async Task<PaginationDTO<PaginatedDimensionDTO>> GetAll(string name, int page)
        {
            var dimensions =
                await _unitOfWork.DimensionRepository.GetAllDimension(name, page, _appConfig.ClientsPerPageFilter);

            var amount = 0;

            if (dimensions.Any())
            {
                amount = dimensions.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<PaginatedDimensionDTO>
            {
                Items = dimensions,
                CurrentPage = page,
                ItemsPerPage = _appConfig.ClientsPerPageFilter,
                TotalOfItems = amount
            };
        }

        public async Task<IEnumerable<DimensionDTO>> GetAllDimensions() =>
            await _unitOfWork.DimensionRepository.GetAllDimension(null, 1, int.MaxValue);

        public async Task<DimensionDTO> GetDimensionById(int dimensionId)
        {
            var dimension = await _unitOfWork.DimensionRepository.GetDimensionById(dimensionId);
            dimension.Domains = await _unitOfWork.DimensionRepository.GetDimensionDomainById(dimensionId);

            return dimension;
        }

        public async Task<PaginationDTO<DimensionLog>> GetDimensionLog(DimensionLogFilter filter)
        {
            var log = (await _unitOfWork.DimensionRepository.GetDimensionLog(filter)).ToList();
            var amount = 0;

            if (log.Any())
            {
                amount = log.FirstOrDefault().TotalOfItems;
            }

            return new PaginationDTO<DimensionLog>
            {
                Items = log,
                CurrentPage = filter.Page,
                ItemsPerPage = filter.PageSize,
                TotalOfItems = amount
            };
        }

        public async Task<ResultDTO<Dimension>> Save(DimensionFormDTO dimension)
        {
            var result = await new DimensionValidationService(_unitOfWork, _stringLocalizer).Validate(dimension);
            var dimensionReturn = new Dimension();

            if (result.IsValid)
            {
                if (dimension.Id == 0)
                {
                    dimensionReturn = await _unitOfWork.DimensionRepository.AddDimension(dimension);
                }
                else
                {
                    dimensionReturn = await _unitOfWork.DimensionRepository.UpdateDimension(dimension);
                    var domains = await _unitOfWork.DimensionRepository.GetDimensionDomainById(dimension.Id);

                    if (domains.Any())
                    {
                        dimensionReturn.Domains = domains.Select(d => new DimensionDomain
                        {
                            DimensionDomainId = d.DimensionDomainId,
                            CreatedDate = d.CreatedDate,
                            DimensionId = d.DimensionId,
                            DomainValue = d.DomainValue,
                            UserId = d.UserId,
                            UserName = d.UserName
                        });
                    }
                }
            }

            return new ResultDTO<Dimension>
            {
                Erros = result.IsValid ? null : result.Errors.Distinct(),
                IndSucesso = result.IsValid,
                Model = dimensionReturn
            };
        }
    }
}
