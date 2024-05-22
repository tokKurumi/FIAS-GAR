var builder = DistributedApplication.CreateBuilder(args);

var elasticsearch = builder.AddContainer("elasticsearch", "elasticsearch", "8.9.1")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithEnvironment("discovery.type", "single-node")
    .WithVolume("gar-elastic-data", "/usr/share/elasticsearch/data")
    .WithVolume("gar-elastic-plugins", "/usr/share/elasticsearch/plugins")
    .WithHttpEndpoint(9200, targetPort: 9200);

var logstash = builder.AddContainer("logstash", "logstash", "8.13.4")
    .WithBindMount("../logstash/logstash.conf", "/usr/share/logstash/pipeline/logstash.conf")
    .WithBindMount("../logstash/script.sql", "/usr/share/logstash/config/queries/script.sql")
    .WithBindMount("../logstash/postgresql-42.2.20.jar", "/usr/share/logstash/postgresql-42.2.20.jar")
    .WithEnvironment("LS_JAVA_OPTS", "-Xmx3g");

var databasePassword = builder.AddParameter("gar-database-postgres-container-password", true);

var database = builder
    .AddPostgres("gar-database-postgres-container", password: databasePassword, port: 5432)
    .WithPgAdmin()
    .WithDataVolume("gar-volume")
    .AddDatabase("gar-database");

var readerApi = builder
    .AddProject<Projects.GAR_Services_ReaderApi>("gar-services-readerapi")
    .WithReference(database);

var searchApi = builder
    .AddProject<Projects.GAR_Services_SearchApi>("gar-services-searchapi");

builder.Build().Run();
