using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Services.Domain;
using Bat.PortalDeCargas.Domain.Services.Files;
using Bat.PortalDeCargas.Resource;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.Dimensions
{
    public class DimensionFileValidateService: IDimensionFileValidateService
    {

        
        private readonly IUnitOfWork UnitOfWork;
        private readonly IFileServiceConstructor FileServiceConstructor;
        private readonly IStringLocalizer<DimensionTranslation> stringLocalizer;


        private IList<string> Erros = new List<string>();

        public DimensionFileValidateService(
            IFileServiceConstructor fileServiceConstructor,
            IUnitOfWork unitOfWork,
            IStringLocalizer<DimensionTranslation> stringLocalizer)
        {            
                        
            this.UnitOfWork = unitOfWork;
            this.FileServiceConstructor = fileServiceConstructor;
            this.stringLocalizer = stringLocalizer;
        }

        public IEnumerable<string> GetErros()
        {
            return this.Erros;
        }

       

        public async Task<MemoryStream> ValidateFile(IFormFile File, int dimensionId)
        {
            var fileService = this.FileServiceConstructor.CreateFileByType(File.ContentType);

            if (!fileService.FileTypeIsValid(File))
            {             
                this.Erros.Add(this.stringLocalizer["FileTypeInvalid"]);
                return null;
            }

            ValidateListDomain ValidateList = null;

            var Dimension =  await this.UnitOfWork.DimensionRepository.GetDimensionById(dimensionId);
            
            var DimensionDomains = await this.UnitOfWork.DimensionRepository.GetDimensionDomainById(dimensionId);

            var mustValidateDomain = DimensionDomains.Any();

            if (mustValidateDomain)
                ValidateList = new ValidateListDomain(DimensionDomains.Select(d => d.DomainValue).ToList());
           
            var Linhas  = fileService.ReadDomainFile(File);

            var ValidateDomainConstructor = new ValidateDomainConstructor();

            var DimensionValidator = ValidateDomainConstructor.CreateValidator(Dimension.DimensionType,this.stringLocalizer);

            var ExportedLines  = new List<RowValidateDTO>();


            foreach(var Linha in Linhas)
            {
                var erros = DimensionValidator.IsValidDomain(Dimension, Linha);

                if (mustValidateDomain)
                    if (ValidateList.IsValueNotInDomain(Linha))
                        erros.Add(string.Format(this.stringLocalizer["ValueNotInDimensionDomain"].Value, Linha));

                ExportedLines.Add(new RowValidateDTO(Linha, erros));
            }

            var resultfile = this.FileServiceConstructor.CreateExelFileService();

            return resultfile.CreateDomainFile(ExportedLines);
        }
    }
}
