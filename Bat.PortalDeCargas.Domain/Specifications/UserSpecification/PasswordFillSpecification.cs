using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.UserSpecification
{
    public class PasswordFillSpecification : SyncSpecification<ChangePasswordDTO>
    {
        public override bool IsSatisfiedBy(ChangePasswordDTO entity)
        {
            return !string.IsNullOrWhiteSpace(entity.Password);
        }

    }
}
