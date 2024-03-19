using GAR.Services.ReaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDataSource("gar-database");
builder.Services.AddSingleton(provider =>
{
    return new ZipXmlReaderService("99", 500);
});
builder.Services.AddHostedService<DataTransferService>();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
