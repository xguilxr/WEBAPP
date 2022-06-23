using Bat.PortalDeCargas.Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public  interface IDimensionDomainUploadService
    {
        public IList<string> ReadDomainFile(IFormFile File);
        public Task<DimensionDTO> SaveDimensionDomain(int dimensionId, int userId, IList<string> Linhas);
        public bool FileTypeIsValid(IFormFile File);
        IEnumerable<string> GetErros();






    }
}
