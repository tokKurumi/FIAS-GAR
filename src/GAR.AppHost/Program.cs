var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddPostgres("gar-database", password: Guid.NewGuid().ToString())
    .WithPgAdmin(containerName: "gar-pgadmin")
    .WithVolumeMount("gar-volume", "/var/lib/postgresql/data");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

builder.Build().Run();
