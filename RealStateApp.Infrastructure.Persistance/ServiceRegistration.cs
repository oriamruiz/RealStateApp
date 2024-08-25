using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Infrastructure.Persistance.Contexts;
using System.Reflection;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Infrastructure.Persistance.Repositories;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Infrastructure.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {

                services.AddDbContext<ApplicationContext>(op => op.UseInMemoryDatabase("DatabaseTest"));


            }
            else
            {
                string stringcon = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(op => op.UseSqlServer(stringcon, m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));


            }


            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<ISaleTypeRepository, SaleTypeRepository>();
            services.AddTransient<IImprovementRepository, ImprovementRepository>();
            services.AddTransient<IPropertyImprovementRepository, PropertyImprovementRepository>();
            services.AddTransient<IFavoritePropertyRepository, FavoritePropertyRepository>();

        }
    }
}
