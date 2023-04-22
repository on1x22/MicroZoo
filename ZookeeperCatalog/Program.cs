using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeeperCatalog;
using MicroZoo.ZookeeperCatalog.DBContext;
using MicroZoo.ZookeeperCatalog.Models;
using MicroZoo.ZookeeperCatalog.Repository;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using MicroZoo.ZookeeperCatalog.Apis;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging(builder => builder.AddConsole());
//builder.Services.AddControllers();

//string connectionString = builder.Configuration.GetConnectionString("ZookeeperDB");
//builder.Services.AddDbContext<ZookeeperDBContext>(c => c.UseNpgsql(connectionString));
//builder.Services.AddTransient<IZookeeperRepository, ZookeeperRepository>();

RegisterServices(builder.Services);

var app = builder.Build();



/*//app.MapGet("/zookeeper/{name:string}", (IZookeeperCatalog zc, string name) =>
app.MapGet("/zookeeper/name/{name}", (IZookeeperRepository zc, string name) =>
{
    var zookeeper = zc.GetByName(name);
    if (zookeeper == null)
        return Results.NotFound();

    return Results.Ok(zookeeper);
});

//app.MapGet("/zookeeper/{id:int}", (IZookeeperCatalog zc, int id) =>
app.MapGet("/zookeeper/id/{id}", (IZookeeperRepository zc, int id) =>
{
    var zookeeper = zc.GetById(id);
    if (zookeeper == null)
        return Results.NotFound(); 
    
    return Results.Ok(zookeeper);       
});

app.MapGet("/zookeeper", (IZookeeperRepository zc) =>
{
    var allZookeepers = zc.GetAll();
    if (allZookeepers == null)
        return Results.NotFound();

    return Results.Ok(allZookeepers);
});

//app.MapGet("/zookeeperspeciality/{speciality:string}", (IZookeeperCatalog zc, string speciality) =>
app.MapGet("/zookeeper/speciality/{speciality}", (IZookeeperRepository zc, string speciality) =>
{
    var zookepers = zc.GetZookeepersSpeciality(speciality);
    if (zookepers == null)
        return Results.NotFound();

    return Results.Ok(zookepers);
});
*/

//app.MapControllers();

Configure(app);

var apis = app.Services.GetServices<IApi>();
foreach(var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found");
    api.Register(app);
}

app.Run();


void RegisterServices(IServiceCollection services)
{
    services.AddLogging(builder => builder.AddConsole());
    services.AddEndpointsApiExplorer();

    services.AddDbContext<ZookeeperDBContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ZookeeperDB"));
    });

    services.AddScoped<IZookeeperRepository, ZookeeperRepository>();
    services.AddTransient<IApi, ZookeeperApi>();
}

void Configure(WebApplication app)
{
    //app.UseHttpsRedirection();
}