using Asp.Versioning;
using Elastic.Clients.Elasticsearch;
using GAR.ServiceDefaults;
using GAR.Services.SearchApi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var elasticsearchConnectionString = builder.Configuration.GetValue<string>("elasticsearch")!;
var elasticClient = new ElasticsearchClient(new Uri(elasticsearchConnectionString));
builder.Services.AddSingleton(elasticClient);

builder.Services.AddSingleton<SearchService>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SearchApi",
        Description = "An ASP.NET Core Web API for searching data in Elasticsearch GAR",
        Contact = new OpenApiContact
        {
            Name = "Yudashkin Oleg",
            Url = new Uri(@"https://github.com/tokKurumi"),
            Email = @"tokkurumi901@gmail.com",
        },
    });
}).AddSwaggerGenNewtonsoftSupport();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc().AddApiExplorer(config =>
{
    config.GroupNameFormat = "'v'V";
    config.SubstituteApiVersionInUrl = true;
});

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
