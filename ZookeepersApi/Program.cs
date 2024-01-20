using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using MicroZoo.ZookeepersApi.Apis;
using Microsoft.AspNetCore.Http.Json;
using MicroZoo.ZookeepersApi.Services;
using System.Reflection;
using MassTransit;
using MicroZoo.ZookeepersApi.Consumers;

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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    services.AddDbContext<ZookeeperDBContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ZookeepersAPI"));
    });

    services.AddScoped<IZookeeperRepository, ZookeeperRepository>();
    services.AddScoped<IZookeeperApiService, ZookeeperApiService>();
    services.AddTransient<IApi, MicroZoo.ZookeepersApi.Apis.ZookeepersApi>();
    services.AddTransient<RequestHelper>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<CheckZokeepersWithSpecialityAreExistConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("zookeepers-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<CheckZokeepersWithSpecialityAreExistConsumer>(context);
            });
        });
    });
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
    app.MapControllers();
}