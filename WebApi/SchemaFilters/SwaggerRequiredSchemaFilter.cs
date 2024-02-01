using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.SchemaFilters;

public class SwaggerRequiredSchemaFilter: ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        foreach (var schemaPropertyProp in schema.Properties)
        {
            if (schemaPropertyProp.Value.Nullable)
                continue;

            schema.Required.Add(schemaPropertyProp.Key);
        }
    }
}