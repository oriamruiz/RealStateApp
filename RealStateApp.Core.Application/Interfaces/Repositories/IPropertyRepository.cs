using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<Property>> GetAllByAgentIdAsync(string Id);

        Task<Property> GetByCodeAsync(string code);
    }
}