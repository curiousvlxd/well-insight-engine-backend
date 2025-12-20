using Scalar.AspNetCore;
using WellInsightEngine.Api.OpenApi;
using WellInsightEngine.Core;
using WellInsightEngine.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
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