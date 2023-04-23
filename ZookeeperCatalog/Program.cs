using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeeperCatalog;
using MicroZoo.ZookeeperCatalog.DBContext;
using MicroZoo.ZookeeperCatalog.Models;
using MicroZoo.ZookeeperCatalog.Repository;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using MicroZoo.ZookeeperCatalog.Apis;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

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
    
}