using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public interface IFileGeneratorService
    {
        MemoryStream Generate(IList fields);
    }

    public interface IFileGeneratorService<T>
    {
        MemoryStream Generate(IList<T> rows, params string[] extraColumn);
    }
}
