using Microsoft.AspNetCore.OpenApi;

namespace WebApi.Extensions;

public static class OpenApiExtensions
{
    extension(OpenApiOptions config)
    {
        public OpenApiOptions CustomSchemaIds(Func<Type, string?> typeSchemaTransformer, bool includeValueTypes = false)
        {
            return config.AddSchemaTransformer((schema, context, _) =>
            {
                // Skip value types and strings
                if (!includeValueTypes &&
                    (context.JsonTypeInfo.Type.IsValueType ||
                     context.JsonTypeInfo.Type == typeof(String) ||
                     context.JsonTypeInfo.Type == typeof(string)))
                {
                    return Task.CompletedTask;
                }

                // Skip if the schema ID is not already set because we don't want to decorate the schema multiple times
                if (schema.Metadata == null || !schema.Metadata.ContainsKey("x-schema-id"))
                {
                    return Task.CompletedTask;
                }

                // transform the typename based on the provided delegate
                var transformedTypeName = typeSchemaTransformer(context.JsonTypeInfo.Type);
                if (string.IsNullOrWhiteSpace(transformedTypeName)) return Task.CompletedTask;

                // Scalar - decorate the models section
                schema.Metadata["x-schema-id"] = transformedTypeName;

                // Swagger and Scalar specific:
                // for Scalar - decorate the endpoint section
                // for Swagger - decorate the endpoint and model sections
                schema.Title = transformedTypeName;

                return Task.CompletedTask;
            });
        }
    }
}