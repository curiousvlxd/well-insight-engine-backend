using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace WellInsightEngine.Api.OpenApi;

internal sealed class StringEnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (type.IsEnum)
        {
            schema.Type = JsonSchemaType.String;
        }

        return Task.CompletedTask;
    }
}