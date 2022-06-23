using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class ExcelFileService : FileService
    {
        public override IList<string> ReadDomainFile(IFormFile File)
        {
            var lines = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var FileReader = new MemoryStream())
            {
                File.CopyTo(FileReader);

                using (ExcelPackage package = new ExcelPackage(FileReader))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    for (int i = 1; i <= worksheet.Dimension.End.Row; i++)
                        if (worksheet.Cells[i, 1].Value != null)
                            lines.Add(worksheet.Cells[i, 1].Value.ToString());
                }
            }

            return lines;
        }


        public override MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stream = new MemoryStream();    
            

            using (ExcelPackage package = new ExcelPackage())
            {

                var worksheet = package.Workbook.Worksheets.Add("Result");
                var RowIndex = 1;

                foreach(var Row in Rows)
                {
                    worksheet.Cells[RowIndex, 1].Value = Row.Value;

                    if (Row.Erros.Any())
                    {
                        worksheet.Cells[RowIndex, 1].Style.Font.Color.SetColor(Color.Red);
                        worksheet.Cells[RowIndex, 2].Value = string.Join(",", Row.Erros);
                    }

                    RowIndex++;
                }

                worksheet.Cells[1, 1, RowIndex, 2].AutoFitColumns();

                package.SaveAs(stream);
            }

            return stream;

        }
    }
}
