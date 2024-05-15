var builder = DistributedApplication.CreateBuilder(args);

var databasePassword = builder.AddParameter("gar-database-postgres-container-password", true);

var database = builder
    .AddPostgres("gar-database-postgres-container", password: databasePassword)
    .WithPgAdmin()
    .WithDataVolume("gar-volume")
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

builder.Build().Run();
