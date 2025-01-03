
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;

namespace BookService.WebApi.Infrastructure.Extensions
{
    public static class SwaggerExtensions
    {
        private static string[] files = new string[]
        {
            Path.Combine(AppContext.BaseDirectory, "BookService.WebApi.xml"),
            Path.Combine(AppContext.BaseDirectory, "BookService.Domain.xml"),
        };
        public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
            });
            return app;
        }

        public static void AddSwaggerWithVersioning(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                return;
            }
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookService API", Version = "v1", });
                c.OperationFilter<DTOFilter>();
                c.AddEnumsWithValuesFixFilters(o =>
                {
                    o.ApplySchemaFilter = true;
                    o.XEnumNamesAlias = "x-enum-varnames";
                    o.XEnumDescriptionsAlias = "x-enum-descriptions";
                    o.ApplyParameterFilter = true;
                    o.ApplyDocumentFilter = true;
                    o.IncludeDescriptions = true;
                    o.IncludeXEnumRemarks = true;
                    o.DescriptionSource = DescriptionSources.XmlComments;
                    o.NewLine = "\n";
                    foreach (var file in files)
                    {
                        o.IncludeXmlCommentsFrom(file);
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: {your token}",
                    Name = "Authorization",
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
        }
        public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
        {
            public void Configure(SwaggerGenOptions options)
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = Assembly.GetCallingAssembly().GetName().Name,
                        Version = description.ApiVersion.ToString()
                    };

                    if (description.IsDeprecated) info.Description += "This API version has been deprecated.";

                    options.SwaggerDoc(description.GroupName, info);
                }
            }

            public void Configure(string name, SwaggerGenOptions options)
            {
                Configure(options);
            }
        }
    }
}
