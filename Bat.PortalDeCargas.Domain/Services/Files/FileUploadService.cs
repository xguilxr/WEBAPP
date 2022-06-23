using Bat.PortalDeCargas.Resource;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Bat.PortalDeCargas.Domain.DTO;

namespace Bat.PortalDeCargas.Domain.Services.Files
{
    public  class FileUploadService:IFileUploadService
    {

        private readonly IFileServiceConstructor FileServiceConstructor;
        private FileService FileService;

        public FileUploadService(IFileServiceConstructor fileServiceConstructor)
        {          
            this.FileServiceConstructor = fileServiceConstructor;
        }

        public void SetFileType(string ContentType)
        {
            if (ContentType == "text/plain")
                this.FileService =  this.FileServiceConstructor.CreateTextFileService();

            if (ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                this.FileService = this.FileServiceConstructor.CreateExelFileService();

            if (ContentType == "application/vnd.ms-excel")
                this.FileService = this.FileServiceConstructor.CreateExelFileService();

        }

        public bool FileTypeIsValid(IFormFile File)
        {
           return ( File.ContentType == "text/plain" ||
                    File.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                    File.ContentType == "application/vnd.ms-excel");          
        }

        public IList<string> ReadDomainFile(IFormFile File)
        {          
            return this.FileService.ReadDomainFile(File);
        }

       public MemoryStream CreateDomainFile(IList<RowValidateDTO> Rows)
       {
            return this.FileService.CreateDomainFile(Rows);
       }




    }
}
