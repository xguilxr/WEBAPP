using Bat.PortalDeCargas.Resource.Translation;
using System;
using System.Collections.Generic;
using System.Text;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Data;
using Microsoft.Extensions.Localization;
using Bat.PortalDeCargas.Domain.Specifications.UserSpecification;
using Bat.PortalDeCargas.Domain.DTO;

namespace Bat.PortalDeCargas.Domain.Services.UserServices
{
    public class UserValidationService : ValidationService<UserFormDTO, UsersTranslation>
    {
        public UserValidationService(IUnitOfWork unitOfWork, IStringLocalizer<UsersTranslation> stringLocalizer) : base(unitOfWork, stringLocalizer)
        {

        }


        protected override void SetValidations()
        {
            AddSpecification(new UserNameFillSpecification(), stringLocalizer["UserNameMustBeFilled"].Value);
            AddSpecification(new UserEmailFillSpecification(), stringLocalizer["UserEmailMustBeFilled"].Value);
            AddSpecification(new UserEmailDuplicateSpecification(this.unitOfWork), stringLocalizer["UserEmailDuplicated"].Value);
            
        }
    }
}
