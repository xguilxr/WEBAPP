using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class TextFileService :FileService
    {
        public override IList<string> ReadDomainFile(IFormFile File)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(File.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    lines.Add(reader.ReadLine());
            }


            return lines;
        }

        public override MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows)
        {
            return new MemoryStream();
        }
    }
}
