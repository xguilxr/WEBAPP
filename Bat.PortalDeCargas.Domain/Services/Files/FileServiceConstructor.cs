using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public class FileServiceConstructor:IFileServiceConstructor
    {
        public  FileService CreateTextFileService()
        {
            return new TextFileService();
        }

        public  FileService CreateExelFileService()
        {
            return new ExcelFileService();
        }

        public FileService CreateFileByType(string ContentType)
        {
            FileService retorno = null;
            
            if (ContentType == "text/plain")
                retorno = this.CreateTextFileService();

            if (ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                retorno = this.CreateExelFileService();

            if (ContentType == "application/vnd.ms-excel")
                retorno = this.CreateExelFileService();

            return retorno;
        }
    }
}
