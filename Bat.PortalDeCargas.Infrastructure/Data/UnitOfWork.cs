using System;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.Repositories;
using Bat.PortalDeCargas.Infrastructure.Repositories;

namespace Bat.PortalDeCargas.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PortalDeCargasContext dbContext;
        private IDimensionRepository dimensionRepository;
        private ITemplateDimensionRepository templateDimensionRepository;
        private ITemplateRepository templateRepository;
        private IUsersRepository usersRepository;

        public UnitOfWork(PortalDeCargasContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ITemplateDimensionRepository TemplateDimensionRepository =>
            templateDimensionRepository ??= new TemplateDimensionRepository(dbContext);

        public void Dispose()
        {
            dbContext?.Dispose();
            dimensionRepository?.Dispose();
            templateDimensionRepository?.Dispose();
            templateRepository?.Dispose();
            usersRepository?.Dispose();
        }

        public IDimensionRepository DimensionRepository => dimensionRepository ??= new DimensionRepository(dbContext);

        public ITemplateRepository TemplateRepository =>
            templateRepository ??= new TemplateRepository(dbContext, TemplateDimensionRepository);

        public IUsersRepository UsersRepository => usersRepository ??= new UsersRepository(dbContext);
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class => new Repository<TEntity>(dbContext);
    }
}
