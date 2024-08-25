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
    public class PropertyImprovementRepository : GenericRepository<PropertyImprovement>, IPropertyImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
