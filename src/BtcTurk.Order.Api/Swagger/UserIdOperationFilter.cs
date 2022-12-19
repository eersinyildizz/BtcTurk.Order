using BtcTurk.Order.Api.Utils;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BtcTurk.Order.Api.Swagger;

public class UserIdOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Constants.Header.UserId,
            Description = "Mock UserId",
            In = ParameterLocation.Header,
            Required = false,   
            Schema = new OpenApiSchema
            {
                Type = "String" ,
                Default = new OpenApiString(Guid.NewGuid().ToString())
            }
        });
    }
}