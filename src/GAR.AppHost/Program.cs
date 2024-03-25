var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddPostgres("gar-database-postgres-container", password: "342cec33-f357-43b4-a6b2-0c097dc48624")
    .WithPgAdmin(containerName: "gar-database-postgres-pgadmin")
    .WithVolumeMount("gar-volume", "/var/lib/postgresql/data")
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

builder.Build().Run();
