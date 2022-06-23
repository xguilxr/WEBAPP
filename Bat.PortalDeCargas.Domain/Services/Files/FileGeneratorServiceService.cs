using System.Collections.Generic;
using System.IO;
using Bat.PortalDeCargas.Domain.Data;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class FileGeneratorServiceService<T> : IFileGeneratorService<T> where T : class
    {
        private readonly IFileGeneratorService<T> _fileGeneratorService;

        public FileGeneratorServiceService(IFileGeneratorService<T> fileGeneratorService)
        {
            _fileGeneratorService = fileGeneratorService;
        }

        public MemoryStream Generate(IList<T> Rows, params string[] extraColumn) => _fileGeneratorService.Generate(Rows);
    }
}
