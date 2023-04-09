using MicroZoo.ZookeeperCatalog;
using MicroZoo.ZookeeperCatalog.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddTransient<IZookeeperCatalog, ZookeeperCatalog>();

var app = builder.Build();

app.MapGet("/", () => "Hello ZookeeperCatalog!");

//app.MapGet("/zookeeper/{name:string}", (IZookeeperCatalog zc, string name) =>
app.MapGet("/zookeeper/name/{name}", (IZookeeperCatalog zc, string name) =>
{
    var zookeeper = zc.GetByName(name);
    if (zookeeper == null)
        return Results.NotFound();

    return Results.Ok(zookeeper);
});

//app.MapGet("/zookeeper/{id:int}", (IZookeeperCatalog zc, int id) =>
app.MapGet("/zookeeper/id/{id}", (IZookeeperCatalog zc, int id) =>
{
    var zookeeper = zc.GetById(id);
    if (zookeeper == null)
        return Results.NotFound(); 
    
    return Results.Ok(zookeeper);       
});

app.MapGet("/zookeeper", (IZookeeperCatalog zc) =>
{
    var allZookeepers = zc.GetAll();
    if (allZookeepers == null)
        return Results.NotFound();

    return Results.Ok(allZookeepers);
});

//app.MapGet("/zookeeperspeciality/{speciality:string}", (IZookeeperCatalog zc, string speciality) =>
app.MapGet("/zookeeper/speciality/{speciality}", (IZookeeperCatalog zc, string speciality) =>
{
    var zookepers = zc.GetZookeepersSpeciality(speciality);
    if (zookepers == null)
        return Results.NotFound();

    return Results.Ok(zookepers);
});

app.Run();
