using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.UserSpecification
{
    public class UserEmailFillSpecification : SyncSpecification<UserFormDTO>
    {
        public override bool IsSatisfiedBy(UserFormDTO entity)
        {
            return !string.IsNullOrWhiteSpace(entity.UserEmail);
        }
    }
}
