using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.DBContext;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Repository;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using MicroZoo.Infrastructure.Apis;
using Microsoft.AspNetCore.Http.Json;
using MicroZoo.Infrastructure.Services;
using System.Reflection;
using MassTransit;
using MicroZoo.Infrastructure.Consumers;
using MicroZoo.ZookeepersApi.Services;
using MicroZoo.ZookeepersApi.Repository;
using ZookeepersApi.Consumers;

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
    services.AddScoped<ISpecialitiesRepository, SpecialitiesRepository>();
    services.AddScoped<IZookeeperApiService, ZookeeperApiService>();
    services.AddScoped<ISpecialitiesService, SpecialitiesService>();
    services.AddTransient<IApi, MicroZoo.Infrastructure.Apis.ZookeepersApi>();
    services.AddTransient<RequestHelper>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<CheckZokeepersWithSpecialityAreExistConsumer>();
        x.AddConsumer<ChangeRelationBetweenZookeeperAndSpecialityConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("zookeepers-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<CheckZokeepersWithSpecialityAreExistConsumer>(context);
                e.ConfigureConsumer<ChangeRelationBetweenZookeeperAndSpecialityConsumer>(context);
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