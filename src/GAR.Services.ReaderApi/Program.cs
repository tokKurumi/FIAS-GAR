using GAR.Services.ReaderApi.Data;
using GAR.Services.ReaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<GarDbContext>("gar-database");
builder.Services.AddSingleton(provider =>
{
    return new ZipXmlReaderService("13", 1_000);
});
builder.Services.AddHostedService<DataTransferService>();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
