var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddPostgres("gar-database-postgres-container", password: builder.CreateStablePassword("database-password"))
    .WithPgAdmin()
    .WithDataVolume("gar-database")
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

builder.Build().Run();
