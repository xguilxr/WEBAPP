using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bat.PortalDeCargas.Domain.Entities;
using OfficeOpenXml;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class ExcelTemplateGenerator : IFileGeneratorService<TemplateDimension>
    {
        public MemoryStream Generate(IList<TemplateDimension> dimensions, params string[] extraColumn)
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Result");

            for (var i = 1; i <= dimensions.OrderBy(d => d.TemplateDimensionOrder).ToList().Count; i++)
            {
                worksheet.Cells[1, i].Value = dimensions[i - 1].TemplateDimensionName;
            }

            var count = dimensions.Count + 1;

            foreach (var columnName in extraColumn)
            {
                worksheet.Cells[1, count++].Value = columnName;
            }

            package.SaveAs(stream);

            return stream;
        }
    }
}
