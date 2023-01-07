using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPIAutores.Utilities
{
    public class AddParameterHATEOAS : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod != "GET")
                return;

            if(operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "HATEOASIncluded",
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
