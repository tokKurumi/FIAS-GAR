var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddPostgres("gar-database-postgres-container")
    .WithPgAdmin()
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

builder.Build().Run();
