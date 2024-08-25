using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Infrastructure.Persistance.Repositories
{
    public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        
    }
}
