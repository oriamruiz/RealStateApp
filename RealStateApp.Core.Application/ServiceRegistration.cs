using RealStateApp.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using System.Reflection;
using MediatR;


namespace RealStateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IAdminHomeService, AdminHomeService>();

        }
    }
}
