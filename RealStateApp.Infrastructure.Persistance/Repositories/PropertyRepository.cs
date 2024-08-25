using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistance.Contexts;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RealStateApp.Infrastructure.Persistance.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Property>> GetAllByAgentIdAsync(string Id)
        {
            var properties = await _dbContext.Properties.Where(p => p.AgentId == Id).Include(p=> p.PropertyType).Include(s=>s.SaleType).Include(i => i.Improvements).ThenInclude(i=>i.Improvement).ToListAsync();

            properties = properties.OrderByDescending(p => p.CreatedAt).ToList();

            return properties;
        }

        public override async Task<List<Property>> GetAllAsync()
        {

            var properties = await base.GetAllAsync();

            properties = await _dbContext.Properties.Include(p => p.PropertyType).Include(s => s.SaleType).Include(i => i.Improvements).ThenInclude(i => i.Improvement).ToListAsync();

            properties = properties.OrderByDescending(p=> p.CreatedAt).ToList();

            return properties;
        }

        public async Task<Property> GetByCodeAsync(string code)
        {
            return await _dbContext.Set<Property>().FirstOrDefaultAsync(p=> p.Code == code);
        }
    }
}
