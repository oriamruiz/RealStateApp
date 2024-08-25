using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistance.Contexts;

namespace RealStateApp.Infrastructure.Persistance.Repositories
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public SaleTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
