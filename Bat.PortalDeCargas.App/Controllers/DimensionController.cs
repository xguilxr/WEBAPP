using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Services.Dimensions;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Bat.PortalDeCargas.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class DimensionController : ControllerBase
    {
        private readonly IDimensionFileValidateService _dimensionFileValidateService;
        private readonly IDimensionService _dimensionService;
        private readonly IDimensionDomainUploadService _dimensionUploadService;
        private readonly ILogger<DimensionController> _logger;
        private readonly IStringLocalizer<DimensionTranslation> _stringLocalizer;

        public DimensionController
        (IDimensionService dimensionService, IDimensionDomainUploadService dimensionUploadService,
            IDimensionFileValidateService dimensionFileValidateService, ILogger<DimensionController> logger,
            IStringLocalizer<DimensionTranslation> stringLocalizer)
        {
            _dimensionService = dimensionService;
            _dimensionUploadService = dimensionUploadService;
            _dimensionFileValidateService = dimensionFileValidateService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        [HttpPost("copy")]
        public async Task<IActionResult> CopyDimension([FromBody] DimensionCopyDTO copy)
        {
            try
            {
                var result = await _dimensionService.CopyDimension(copy.Id, copy.Name);

                return Ok(new
                {
                    IndSucesso = result.IsValid,
                    Erros = result.Errors.Distinct()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro copiando a dimensão");

                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value, out var userId);
                var result = await _dimensionService.Delete(id, userId);

                return result.IsValid ? Ok() : (IActionResult)NotFound(result.Errors.Distinct());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro removendo a dimensão");

                throw;
            }
        }

        [HttpDelete("deleteDomain/{id:int}")]
        public async Task<IActionResult> DeleteDomain([FromRoute] int id)
        {
            try
            {
                var result = await _dimensionService.DeleteDimensionDomain(id);

                return result.IsValid ? Ok() : (IActionResult)NotFound(result.Errors.Distinct());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro removendo o domínio");

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDimensions()
        {
            try
            {
                return Ok(await _dimensionService.GetAllDimensions());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo as dimensões");

                throw;
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name, int page)
        {
            try
            {
                return Ok(await _dimensionService.GetAll(name, page));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo dimensão");

                throw;
            }
        }

        [HttpGet]
        [Route("GetDimensionById/{dimensionId:int}")]
        public async Task<IActionResult> GetDimensionById(int dimensionId)
        {
            try
            {
                return Ok(await _dimensionService.GetDimensionById(dimensionId));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo dimensão");

                throw;
            }
        }

        [HttpPost]
        [Route("getDimensionFile")]
        public async Task<IActionResult> GetDimensionFile([FromForm] string name)
        {
            try
            {
                var stream = await _dimensionService.GenerateExcelFile(name);

                return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Dimension.xlsx"
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo arquivo de dimensão");

                throw;
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered([FromQuery] string name, [FromQuery] int page = 1)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(v => v.Errors.FirstOrDefault()));
                }

                return Ok(await _dimensionService.GetAll(name, page));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo dimensões");

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DimensionFormDTO dimension)
        {
            try
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value, out var userId);
                dimension.UserId = userId;

                return Ok(await _dimensionService.Save(dimension));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro criando dimensão");

                throw;
            }
        }

        [HttpPost]
        [Route("uploadDomain")]
        public async Task<IActionResult> UploadDomain([FromForm] IFormFile file, [FromForm] string dimensionId)
        {
            if (!_dimensionUploadService.FileTypeIsValid(file))
            {
                return Ok(new ResultDTO
                {
                    IndSucesso = false,
                    Erros = _dimensionUploadService.GetErros()
                });
            }

            DimensionDTO dimension;

            try
            {
                var linhas = _dimensionUploadService.ReadDomainFile(file);
                dimension = await _dimensionUploadService.SaveDimensionDomain(Convert.ToInt32(dimensionId), 1, linhas);
            }
            catch (Exception ex)
            {
                var erros = new List<string>();
                erros.Add(ex.Message);

                return Ok(new ResultDTO
                {
                    IndSucesso = false,
                    Erros = erros
                });
            }

            return Ok(new ResultDTO<DimensionDTO>
            {
                IndSucesso = true,
                Model = dimension
            });
        }

        [HttpPost]
        [Route("uploadTest")]
        public async Task<IActionResult> UploadTest([FromForm] IFormFile file, [FromForm] string dimensionId)
        {
            try
            {
                var stream = await _dimensionFileValidateService.ValidateFile(file, Convert.ToInt32(dimensionId));

                return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "criticas.xlsx"
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro submetendo arquivo de teste de dimensão");

                throw;
            }
        }

        [HttpGet("getDimensionLog")]
        public async Task<IActionResult> GetDimensionLog([FromQuery] DimensionLogFilter filter)
        {
            try
            {
                return Ok(await _dimensionService.GetDimensionLog(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo logs de dimensão");

                throw;
            }
        }
    }
}
