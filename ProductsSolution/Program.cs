using Asp.Versioning;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Common;
using ProductsSolution.Common.Services;
using ProductsSolution.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddFastEndpoints();
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IPostProcessor<,>), typeof(ExecutionTimePostProcessor<,>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v1";
        s.Title = " Products API(Staging slot version)";
        s.Version = "v1";
    };
    o.MaxEndpointVersion = 1;
});

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v2";
        s.Title = "Products API";
        s.Version = "v2";
    };
    o.MinEndpointVersion = 2;
    o.MaxEndpointVersion = 2;
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-API-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});
var app = builder.Build();

app.UseSwaggerGen();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;

    c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return TypedResults.BadRequest(new ProductsSolution.Common.ErrorResponse
        {
            Message = "Validation failed",
            Errors = failures.Select(f => new ErrorItem
            {
                Field = f.PropertyName,
                Message = f.ErrorMessage
            }).ToList()
        });
    };
});

app.MapControllers();
app.Run();