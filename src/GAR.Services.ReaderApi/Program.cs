using GAR.Services.ReaderApi.Data;
using GAR.Services.ReaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider =>
{
    return new ZipXmlReaderService("13", 1_000);
});
builder.AddNpgsqlDataSource("gar-database");
builder.Services.AddSingleton<DataMapHelper>();
builder.Services.AddScoped<DataWriterService>();
builder.Services.AddHostedService<DatabaseInitializerService>();
builder.Services.AddHostedService<DataTransferService>();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
