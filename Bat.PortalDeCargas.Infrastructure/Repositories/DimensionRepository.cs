using System.Collections.Generic;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bat.PortalDeCargas.Infrastructure.Repositories
{
    public class DimensionRepository : Repository<Dimension>, IDimensionRepository
    {
        internal DimensionRepository(PortalDeCargasContext dbContext) : base(dbContext)
        { }

        public async Task<Dimension> AddDimension(DimensionFormDTO dimension)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await Add(new
                {
                    DimensionName = dimension.Name,
                    DimensionType = dimension.Type,
                    DimensionSize = dimension.Size,
                    DimensionFormat = dimension.Format,
                    CreatedUserId = dimension.UserId,
                    DimensionStartNumber = dimension.StartNumber,
                    DimensionEndNumber = dimension.EndNumber,
                    DimensionDescription = dimension.Description
                }, tran.GetDbTransaction());

                await Execute("up_InsertDimensionLog", new
                {
                    dimension.UserId,
                    result.DimensionId,
                    action = LogAction.Create
                }, tran.GetDbTransaction());

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public Task<int> AddDimensionDomain(IEnumerable<DimensionDomainDTO> dimensionsDomain)
        {
            var sql =
                @"insert into DimensionDomain (DomainValue,DimensionId,CreatedDate,UserId ) values (@DomainValue,@DimensionId,@CreatedDate,@UserId )";

            return ExcecutRange(sql, dimensionsDomain);
        }

        public Task<int> Copy(int dimensionId, string dimensionName)
        {
            var userId = 1;

            return Execute("up_CopyDimension", new
            {
                DimensionId = dimensionId,
                CreatedUserId = userId,
                DimensionName = dimensionName
            });
        }

        public async Task<int> Delete(int dimensionId, int userId)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await Remove(new
                {
                    dimensionId
                }, tran.GetDbTransaction());

                await Execute("up_InsertDimensionLog", new
                {
                    userId,
                    dimensionId,
                    action = LogAction.Delete
                }, tran.GetDbTransaction());

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public Task<int> DeleteDimensionDomain(int idDimension)
        {
            var sql = "delete from DimensionDomain where DimensionId = @DimensionId";

            return ExecuteComand(sql, new
            {
                DimensionId = idDimension
            });
        }

        public Task<IEnumerable<PaginatedDimensionDTO>> GetAllDimension(string name, int page, int pageCount)
        {
            object parameters = new
            {
                DimensionName = name,
                CurrentPage = page,
                ItemsPerPage = pageCount
            };

            return Query<PaginatedDimensionDTO>("up_GetAllDimension", parameters);
        }

        public Task<IEnumerable<DimensionFilterDTO>> GetDimensionByFilter(string name)
        {
            object parameters = new
            {
                DimensionName = name
            };

            return Query<DimensionFilterDTO>("up_GetDimensionByFilter", parameters);
        }

        public Task<DimensionDTO> GetDimensionById(int id)
        {
            object parameters = new
            {
                DimensionId = id
            };

            return FirstOrDefault<DimensionDTO>("up_GetDimension", parameters);
        }

        public Task<IEnumerable<DimensionDTO>> GetDimensionByName(string name)
        {
            object parameters = new
            {
                DimensionName = name
            };

            return Query<DimensionDTO>("up_GetDimension", parameters);
        }

        public Task<IEnumerable<DimensionDomainDTO>> GetDimensionDomainById(int idDimension)
        {
            var sql =
                "select DimensionDomainId,DomainValue,DimensionId,CreatedDate,a.UserId,UserName from DimensionDomain a inner join Users b on a.UserId = b.UserId where DimensionId = @DimensionId";

            return QueryComand<DimensionDomainDTO>(sql, new
            {
                DimensionId = idDimension
            });
        }

        public async Task<Dimension> UpdateDimension(DimensionFormDTO dimension)
        {
            await using var tran = await DbContext.Database.BeginTransactionAsync();

            try
            {
                await Execute("up_InsertDimensionLog", new
                {
                    dimension.UserId,
                    DimensionId = dimension.Id,
                    action = LogAction.Update
                }, tran.GetDbTransaction());

                var result = await Update<Dimension>(new
                {
                    DimensionId = dimension.Id,
                    DimensionName = dimension.Name,
                    DimensionType = dimension.Type,
                    DimensionSize = dimension.Size,
                    DimensionFormat = dimension.Format,
                    DimensionStartNumber = dimension.StartNumber,
                    DimensionEndNumber = dimension.EndNumber,
                    UpdatedUserId = dimension.UserId,
                    DimensionDescription = dimension.Description
                }, tran.GetDbTransaction());

                await tran.CommitAsync();

                return result;
            }
            catch
            {
                await tran.RollbackAsync();

                throw;
            }
        }

        public async Task<IEnumerable<DimensionLog>> GetDimensionLog(DimensionLogFilter filter)
        {
            return await Query<DimensionLog>("up_GetDimensionLog", filter);
        }
    }
}
