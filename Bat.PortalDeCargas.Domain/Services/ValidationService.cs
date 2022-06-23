using Bat.PortalDeCargas.Domain.Data;
using Impeto.Framework.Domain.Service;
using Impeto.Framework.Domain.Specification;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services
{
    public abstract class ValidationService<T,M> where T : class
    {
        private readonly IList<ValidationMessagePair<T>> validationMessagePairs = new List<ValidationMessagePair<T>>();
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IStringLocalizer<M> stringLocalizer;

        protected ValidationService()
        {
            SetValidations();
        }

        public ValidationService(IUnitOfWork unitOfWork, IStringLocalizer<M> stringLocalizer)
        {
            this.unitOfWork = unitOfWork;
            this.stringLocalizer = stringLocalizer;
            SetValidations();
        }

        protected abstract void SetValidations();

        protected void AddValidation(Func<T, Task<bool>> validation, Func<T, string> messageExpression)
        {
            validationMessagePairs.Add(new ValidationMessagePair<T>
            {
                MessageExpression = messageExpression,
                Validation = validation
            });
        }

        protected void AddSpecification(ISpecification<T> specification, Func<T, string> messageExpression)
        {
            AddValidation(entity => specification.IsSatisfiedBy(entity), messageExpression);
        }

        protected void AddSpecification(ISpecification<T> specification, string message)
        {
            AddSpecification(specification, entity => message);
        }

        public virtual async Task<ValidationResult> Validate(T entity)
        {
            var errors = new List<string>();
            foreach (var specificationMessagePair in validationMessagePairs)
            {
                if (!await specificationMessagePair.Validation(entity))
                    errors.Add(specificationMessagePair.MessageExpression(entity));
            }

            return new ValidationResult(errors);
        }
    }
}
