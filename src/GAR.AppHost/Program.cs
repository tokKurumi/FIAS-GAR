var builder = DistributedApplication.CreateBuilder(args);

var elasticsearch = builder.AddContainer("elasticsearch", "elasticsearch", "8.13.0")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithEnvironment("discovery.type", "single-node")
    .WithVolume(target: "/usr/share/elasticsearch/data")
    .WithHttpEndpoint(9200, targetPort: 9200);

var databasePassword = builder.AddParameter("gar-database-postgres-container-password", true);

var database = builder
    .AddPostgres("gar-database-postgres-container", password: databasePassword)
    .WithPgAdmin()
    .WithDataVolume("gar-volume")
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

var searchApi = builder
    .AddProject<Projects.GAR_Services_SearchApi>("gar-services-searchapi");

builder.Build().Run();
