using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public abstract class FileService
    {
        public abstract IList<string> ReadDomainFile(IFormFile File);

        public abstract MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows);

        public bool FileTypeIsValid(IFormFile File)
        {
            return (File.ContentType == "text/plain" ||
                     File.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                     File.ContentType == "application/vnd.ms-excel");
        }
    }
}
