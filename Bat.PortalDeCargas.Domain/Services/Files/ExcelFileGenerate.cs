using System;
using System.Collections.Generic;
using System.IO;
using Bat.PortalDeCargas.Domain.Data;
using OfficeOpenXml;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class ExcelFileGenerate<T> : IFileGeneratorService<T> where T : class
    {
        public MemoryStream Generate(IList<T> Rows, params string[] extraColumn)
        {
            var fields = typeof(T).GetProperties();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            object value = "";
            var columnName = "";

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Result");
                var RowIndex = 1;
                var type = "";

                for (var ColIndex = 0; ColIndex < fields.Length; ColIndex++)
                {
                    columnName = fields[ColIndex].Name;
                    worksheet.Cells[RowIndex, ColIndex + 1].Value = columnName;
                }

                RowIndex++;

                foreach (var Row in Rows)
                {
                    for (var ColIndex = 0; ColIndex < fields.Length; ColIndex++)
                    {
                        value = fields[ColIndex].GetValue(Row);

                        if (value != null)
                        {
                            type = fields[ColIndex].PropertyType.Name;

                            if (type.ToUpper().Contains("INT"))
                            {
                                worksheet.Cells[RowIndex, ColIndex + 1].Value = Convert.ToInt32(value);
                            }
                            else
                            {
                                worksheet.Cells[RowIndex, ColIndex + 1].Value = value.ToString();
                            }
                        }
                    }

                    RowIndex++;
                }

                worksheet.Cells[1, 1, RowIndex, fields.Length + 1].AutoFitColumns();
                package.SaveAs(stream);
                //package.SaveAs(@"D:\Projetos\SouzaCruz\draft\template.xslx");
            }

            return stream;
        }
    }
}
