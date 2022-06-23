using System.Collections.Generic;
using System.Linq;
using Bat.PortalDeCargas.Domain.Extensions;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateValidationResult
    {
        private readonly List<TemplateRowValidationResult> _rowValidationResult;
        private readonly List<string> _templateErrors;

        public TemplateValidationResult(int templateId, string fileName)
        {
            TemplateId = templateId;
            FileName = fileName;
            _rowValidationResult = new List<TemplateRowValidationResult>();
            _templateErrors = new List<string>();
        }

        public string FileName { get; }
        public IEnumerable<TemplateRowValidationResult> Rows => _rowValidationResult;
        public byte[] Stream { get; set; }
        public IEnumerable<string> TemplateErrors => _templateErrors.AsReadOnly();
        public int TemplateId { get; }
        public int UploadLogId { get; set; }
        public bool IsValid => _rowValidationResult.All(r => r.isValid) && _templateErrors.IsEmpty();

        public void AddRowValidationError(TemplateRowValidationResult validationResult)
        {
            _rowValidationResult.Add(validationResult);
        }

        public void AddTemplateError(string error)
        {
            _templateErrors.Add(error);
        }
    }
}
