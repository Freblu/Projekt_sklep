using PartsCom.AppHost;
using Microsoft.Extensions.Configuration;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> minioRootUserParam = builder.AddParameter("MinioRootUser", secret: true);
IResourceBuilder<ParameterResource> minioRootPasswordParam = builder.AddParameter("MinioRootPassword", secret: true);

IResourceBuilder<SqlServerDatabaseResource> database = builder
    .AddSqlServer("PartsComDb")
    .WithDataVolume("PartsComDataVolume")
    .AddDatabase("PartsCom");

IResourceBuilder<MinioContainerResource> minio = builder.AddMinioContainer("partscom-minio", minioRootUserParam, minioRootPasswordParam)
    .WithDataVolume("minio-data");

builder.AddProject<Projects.PartsCom_Ui>("partscom-ui") 
    .WithReference(database)
    .WithReference(minio)
    .WaitFor(database)
    .WaitFor(minio)
    .WithEnvironmentFromConfiguration("PartsCom");

await builder.Build().RunAsync();
