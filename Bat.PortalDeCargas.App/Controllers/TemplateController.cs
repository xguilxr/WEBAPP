using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Services.Template;
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
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly IStringLocalizer<TemplateTranslation> _stringLocalizer;
        private readonly ITemplateService _templateService;

        public TemplateController
        (ITemplateService templateService, ILogger<TemplateController> logger,
            IStringLocalizer<TemplateTranslation> stringLocalizer)
        {
            _templateService = templateService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        [HttpDelete]
        [Route("{templateId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int templateId)
        {
            try
            {
                return Ok(await _templateService.Delete(templateId,
                    int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro removendo template");

                throw;
            }
        }

        [HttpPost]
        [Route("{templateId:int}/getFile")]
        public async Task<IActionResult> GetFile(int templateId)
        {
            MemoryStream stream = null;

            try
            {
                string fileName;
                string contentType;
                (stream, fileName, contentType) = await _templateService.GenerateFile(templateId);

                return new FileContentResult(stream.ToArray(), contentType)
                {
                    FileDownloadName = fileName
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo arquivo de template");

                throw;
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered
            ([FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int pageCount = 5)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(v => v.Errors.FirstOrDefault()));
                }

                return Ok(await _templateService.GetAll(name, page, pageCount));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo templates");

                throw;
            }
        }

        [HttpGet]
        [Route("GetTemplateById/{templateId:int}")]
        public async Task<IActionResult> GetTemplateById(int templateId)
        {
            try
            {
                return Ok(await _templateService.GetTemplateById(templateId));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo template");

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] TemplateFormDTO template)
        {
            try
            {
                template.UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

                return Ok(await _templateService.Save(template));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro criando o template");

                throw;
            }
        }

        [HttpGet("upload/{templateId:int}")]
        public async Task<IActionResult> GetLastUpload(int templateId)
        {
            try
            {
                var result = await _templateService.GetLastUpload(templateId);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro submetendom o arquivo do template");

                throw;
            }
        }

        [HttpGet("upload/detail")]
        public async Task<IActionResult> GetUploadDetail([FromQuery] FileDetailFilter filter)
        {
            try
            {
                var result = await _templateService.GetUploadDetail(filter);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo os detalhes do upload");

                throw;
            }
        }

        [HttpPost]
        [Route("{templateId:int}/validateFile")]
        public async Task<IActionResult> ValidateFile(int templateId, [FromForm] IFormFile file)
        {
            try
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
                var result = await _templateService.UploadFile(file, templateId, userId);

                if (result.TemplateErrors.Any())
                {
                    return BadRequest(result.TemplateErrors);
                }

                return new FileContentResult(result.Stream, file.ContentType)
                {
                    FileDownloadName = result.FileName
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro validando o upload");

                throw;
            }
        }

        [HttpGet("getTemplateLog")]
        public async Task<IActionResult> GetTemplateLog([FromQuery] TemplateLogFilter filter)
        {
            try
            {
                return Ok(await _templateService.GetTemplateLog(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo log de templates");

                throw;
            }
        }

        [HttpGet("getUploadLog")]
        public async Task<IActionResult> GetUploadLog([FromQuery] UploadLogFilter filter)
        {
            try
            {
                return Ok(await _templateService.GetUploadLog(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo log de upload");

                throw;
            }
        }
    }
}
