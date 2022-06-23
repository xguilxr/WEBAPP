using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.Entities;
using Impeto.Framework.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bat.PortalDeCargas.Domain.DTO;

namespace Bat.PortalDeCargas.Domain.Specifications.UserSpecification
{
    public class UserEmailDuplicateSpecification : SyncSpecification<UserFormDTO>
    {
        private IUnitOfWork unitOfWork;

        public UserEmailDuplicateSpecification(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;  
        }

        public override bool IsSatisfiedBy(UserFormDTO entity)
        {
            var user = unitOfWork.UsersRepository.GetUserByEmail(entity.UserEmail).Result;
            return user == null;
        }
    }
}
