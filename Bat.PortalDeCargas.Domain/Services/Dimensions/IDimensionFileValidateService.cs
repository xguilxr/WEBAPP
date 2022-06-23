using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public interface IDimensionFileValidateService
    {
        Task<MemoryStream> ValidateFile(IFormFile File, int dimensionId);
        IEnumerable<string> GetErros();
     
    }
}
