using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Repositories;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.Services.Files;
using System.Collections.ObjectModel;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Domain.Entities;
using Microsoft.Extensions.Localization;
using Bat.PortalDeCargas.Resource.Translation;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public class DimensionDomainUploadService : IDimensionDomainUploadService
    {

        private readonly IUnitOfWork unitOfWork;              
        private readonly IFileServiceConstructor FileServiceConstructor;
        private readonly IStringLocalizer<DimensionTranslation> stringLocalizer;

        private IList<string> Erros = new List<string>();


        public DimensionDomainUploadService(IUnitOfWork unitOfWork,
              IFileServiceConstructor fileServiceConstructor,
               IStringLocalizer<DimensionTranslation> stringLocalizer)
        {
            this.unitOfWork = unitOfWork;            
            this.FileServiceConstructor = fileServiceConstructor;
            this.stringLocalizer = stringLocalizer;
        }


        public IEnumerable<string> GetErros()
        {
            return this.Erros;
        }

        public bool FileTypeIsValid(IFormFile File)
        {
            var fileService = this.FileServiceConstructor.CreateFileByType(File.ContentType);
            
            var fileIsValid = fileService.FileTypeIsValid(File);
            
            if (!fileIsValid)
                this.Erros.Add(this.stringLocalizer["FileTypeInvalid"].Value);

            return fileIsValid;
        }

        public IList<string> ReadDomainFile(IFormFile File)
        {
            var fileService = this.FileServiceConstructor.CreateFileByType(File.ContentType);

            return fileService.ReadDomainFile(File);
        }

        public async Task<DimensionDTO> SaveDimensionDomain(int dimensionId, int userId, IList<string> Linhas)
        {
            var Domains = Linhas.Select(value => GetDimensionDomain(value, dimensionId, userId)); 

            await this.unitOfWork.DimensionRepository.DeleteDimensionDomain(dimensionId);
            
            await this.unitOfWork.DimensionRepository.AddDimensionDomain(Domains);

            var dimension = await this.unitOfWork.DimensionRepository.GetDimensionById(dimensionId);

            dimension.Domains = await this.unitOfWork.DimensionRepository.GetDimensionDomainById(dimensionId);

            return dimension;
        }


        private  DimensionDomainDTO GetDimensionDomain (string value, int dimensionId, int userId)
        {
            return new DimensionDomainDTO()
            {
                CreatedDate = DateTime.Now,
                DimensionId = dimensionId,
                DomainValue = value,
                UserId = 1
            };
        }
    }
}
