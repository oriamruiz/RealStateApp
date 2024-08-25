using RealStateApp.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;


namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsyncForWeb(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.CLIENTE.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.AGENTE.ToString()));
            

        }

        public static async Task SeedAsyncForApi(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.DESARROLLADOR.ToString()));

        }
    }
}
