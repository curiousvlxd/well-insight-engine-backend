using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using WellInsightEngine.Api.OpenApi;
using WellInsightEngine.Core;
using WellInsightEngine.Core.Converters;
using WellInsightEngine.Infrastructure;
using WellInsightEngine.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    o.KnownIPNetworks.Clear();
    o.KnownProxies.Clear();
});
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {   
        o.JsonSerializerOptions.Converters.Add(new DescriptionEnumJsonConverter());
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
   );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddSchemaTransformer<StringEnumSchemaTransformer>();
});
builder.Services.AddInfrastructure();
builder.Services.AddCore();

var app = builder.Build();
await app.MigrateIfNeededAsync();
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseCors(policy =>
{
    policy
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "WellInsightEngine API";
    options.Theme = ScalarTheme.Purple;
});

app.Run();