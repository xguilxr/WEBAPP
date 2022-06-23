using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public interface IFileUploadService
    {
        public bool FileTypeIsValid(IFormFile File);
        public IList<string> ReadDomainFile(IFormFile File);
        void SetFileType(string ContentType);
        MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows);

    }
}
