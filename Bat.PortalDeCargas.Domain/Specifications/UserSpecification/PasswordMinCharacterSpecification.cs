using Bat.PortalDeCargas.Domain.DTO;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Specifications.UserSpecification
{
    public class PasswordMinCharacterSpecification : SyncSpecification<ChangePasswordDTO>
    {
        private int Size;

        public PasswordMinCharacterSpecification(int size):base()
        {
            this.Size = size;
        }
        public override bool IsSatisfiedBy(ChangePasswordDTO entity)
        {
            return entity.Password.Length >= this.Size;
        }

    }
}
