using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public interface IFileServiceConstructor
    {
        FileService CreateTextFileService();

        FileService CreateExelFileService();

        FileService CreateFileByType(string ContentType);
        
    }
}
