using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Specifications.UserSpecification;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public class PasswordValidationService : ValidationService<ChangePasswordDTO, UsersTranslation>
    {
        const int MIN_NUMBER_CHAR = 6;

        public PasswordValidationService(IUnitOfWork unitOfWork, IStringLocalizer<UsersTranslation> stringLocalizer) : base(unitOfWork, stringLocalizer)
        {

        }


        protected override void SetValidations()
        {
            AddSpecification(new PasswordFillSpecification(), stringLocalizer["PasswordMustBeFilled"].Value);
            AddSpecification(new PasswordEqualConfirmSpecification(), stringLocalizer["PasswordEqConfirmation"].Value);
            AddSpecification(new PasswordMinCharacterSpecification(MIN_NUMBER_CHAR), string.Format(stringLocalizer["PassowrdMinChar"].Value,MIN_NUMBER_CHAR));

        }
    }
}
