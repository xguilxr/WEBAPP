using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public interface  IFileService
    {
        public IList<string> ReadDomainFile(IFormFile File);

        public MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows);

       

    }
}
