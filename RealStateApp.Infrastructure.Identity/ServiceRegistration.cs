using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using RealStateApp.Infrastructure.Identity.Contexts;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Infrastructure.Identity.Services;
using RealStateApp.Infrastructure.Identity.Seeds;
using RealStateApp.Core.Application.Interfaces.Services;
using System.Reflection;
using RealStateApp.Core.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using RealStateApp.Core.Application.Dtos.Account;

namespace RealStateApp.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityLayerForApi(this IServiceCollection services, IConfiguration configuration)
        {
            ContextConfiguration(services, configuration);

            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();


            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                //options.ReturnUrlParameter = "";
                options.AccessDeniedPath = "/User/AccessDenied";
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        if (!c.Response.HasStarted)
                        {
                            c.HandleResponse();
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new JwtResponse
                            {
                                HasError = true,
                                Error = "no estas autorizado"
                            });

                            return c.Response.WriteAsync(result);
                        }
                        return Task.CompletedTask;
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponse
                        {
                            HasError = true,
                            Error = "no estas autorizado para acceder a este recurso"
                        });

                        return c.Response.WriteAsync(result);
                    }
                };
            });

            #endregion

            ServiceConfiguration(services);

        }

        public static void AddIdentityLayerForWeb(this IServiceCollection services, IConfiguration configuration)
        {
            ContextConfiguration(services, configuration);

            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                //options.ReturnUrlParameter = "";
                options.AccessDeniedPath = "/User/AccessDenied";
            });


            services.AddAuthentication();

            #endregion

            ServiceConfiguration(services);

        }

        public async static Task AddIdentitySeedsForWeb(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsyncForWeb(roleManager);
                    await DefaultAdminUser.SeedAsync(userManager);
                    await DefaultClientUser.SeedAsync(userManager);
                    await DefaultAgentUser.SeedAsync(userManager);

                }
                catch (Exception ex)
                {
                }


            }


        }

        public async static Task AddIdentitySeedsForApi(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsyncForApi(roleManager);
                    await DefaultDeveloperUser.SeedAsync(userManager);

                }
                catch (Exception ex)
                {
                }


            }


        }

        #region private methods

        private static void ContextConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {

                services.AddDbContext<IdentityContext>(op => op.UseInMemoryDatabase("DatabaseTest"));


            }
            else
            {
                string stringcon = configuration.GetConnectionString("IdentityConnection");
                services.AddDbContext<IdentityContext>(op => op.UseSqlServer(stringcon, m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));


            }

            #endregion
        }

        private static void ServiceConfiguration(IServiceCollection services)
        {
            #region Services

            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }

        #endregion


    }
}
