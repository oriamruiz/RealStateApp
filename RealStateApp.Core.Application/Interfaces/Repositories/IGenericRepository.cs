using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync (Entity entity);

        Task<Entity> UpdateAsync(Entity entity, int id);

        Task DeleteAsync(Entity entity);

        Task<List<Entity>> GetAllAsyncWithInclude(List<string> properties);

        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllAsync();

    }
    
}
