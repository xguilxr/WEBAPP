using System.Collections.Generic;
using System.Linq;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Extensions;

namespace Bat.PortalDeCargas.Domain.Services.Template
{
    public class TemplateValidationCellResult
    {
        public TemplateValidationCellResult()
        {
            Errors = new List<string>();
        }

        public DimensionDTO Dimension { get; set; }
        public string ColumnName { get; set; }
        public int ColumnOrder { get; set; }
        public IList<string> Errors { get; set; }
        public int TemplateDimensionId { get; set; }
        public string Value { get; set; }
        public bool IsValid => Errors.IsEmpty();
        public override string ToString() => $"{ColumnName}: {(Errors.Any() ? string.Join("\n\t", Errors) : "OK")}";
    }
}
