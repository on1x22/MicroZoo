using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using MicroZoo.ZookeepersApi.Apis;
using Microsoft.AspNetCore.Http.Json;
using MicroZoo.ZookeepersApi.Services;

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
    services.AddHttpClient();
    services.AddLogging(builder => builder.AddConsole());
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<ZookeeperDBContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ZookeepersAPI"));
    });

    services.AddScoped<IZookeeperRepository, ZookeeperRepository>();
    services.AddScoped<IZookeeperApiService, ZookeeperApiService>();
    services.AddTransient<IApi, ZookeeperApi>();
    services.AddTransient<RequestHelper>();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ZookeeperDBContext>();
        db.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();
}