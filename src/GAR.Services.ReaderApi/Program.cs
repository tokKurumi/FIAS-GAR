using GAR.Services.ReaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDataSource("gar-database");
builder.Services.AddSingleton(provider =>
{
    return new ZipXmlReaderService("13", 1_000);
});
builder.Services.AddScoped<PostgresDataWriterService>();
builder.Services.AddHostedService<DatabaseInitializerService>();
builder.Services.AddHostedService<DataTransferService>();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
