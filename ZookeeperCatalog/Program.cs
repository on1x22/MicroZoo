using MicroZoo.ZookeeperCatalog;
using MicroZoo.ZookeeperCatalog.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddTransient<IZookeeperCatalog, ZookeeperCatalog>();

var app = builder.Build();

app.MapGet("/", () => "Hello ZookeeperCatalog!");

app.MapGet("/get/{name:string}", (IZookeeperCatalog zc, string name) =>
{
    Results.Ok( zc.Get(name));
});

app.MapGet("/get/{id:int}", (IZookeeperCatalog zc, int id) =>
{
    Results.Ok(zc.Get(id));
});

app.MapGet("/get", (IZookeeperCatalog zc) =>
{
    Results.Ok(zc.Get());
});

app.MapGet("/getzookeepers/{speciality:string}", (IZookeeperCatalog zc, string speciality) =>
{
    Results.Ok(zc.GetZookeepers(speciality));
});

app.Run();
