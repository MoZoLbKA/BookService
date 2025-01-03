using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace BookService.WebApi.Infrastructure.Extensions;

public sealed class DTOFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes(typeof(DTOAttribute), false)?.FirstOrDefault() is DTOAttribute dtoAttribute)
        {
            var schema = context.SchemaGenerator.GenerateSchema(dtoAttribute.Type, context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["application/json"] = new OpenApiMediaType { Schema = schema } }
            };
        }
    }
}
