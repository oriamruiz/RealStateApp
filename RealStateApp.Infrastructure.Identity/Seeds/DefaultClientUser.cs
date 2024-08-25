using RealStateApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new() {
                UserName = "LuisTave",
                Email = "Luis05@gmail.com",
                FirstName = "Luis",
                LastName = "Taveras",
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            if(userManager.Users.All(u=> u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if(user == null) 
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.CLIENTE.ToString());
                }

            }

        }
    }
}
