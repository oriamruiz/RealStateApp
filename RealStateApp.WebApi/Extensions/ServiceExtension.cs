using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace RealStateApp.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                List<string> xmlfiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlfiles.ForEach(xmlfile => options.IncludeXmlComments(xmlfile));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RealStateApp API",
                    Description = "Esta api sera una fuente de distribucion de datos ",
                    Contact = new OpenApiContact
                    {
                        Name = "Oriam Ruiz",
                        Email = "Oriam2227@gmail.com"
                    }

                });

                options.EnableAnnotations();

                options.DescribeAllParametersInCamelCase();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Digita tu token en este formato - Bearer {Tu token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header


                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;


            });
        }
    }
}
