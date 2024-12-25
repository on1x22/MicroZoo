using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MassTransit;
using MicroZoo.ZookeepersApi.Services;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Apis;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.ZookeepersApi.Consumers.Jobs;
using MicroZoo.ZookeepersApi.Consumers.Specialities;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

var apis = app.Services.GetServices<IApi>();
foreach(var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found");
    api.Register(app);
}

app.Run();


void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    /*var animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
    var personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
    var zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);

    services.AddScoped<IConnectionService>(s => new ConnectionService(animalsApiUrl,
                                                                          personsApiUrl, 
                                                                          zookeepersApiUrl));*/
    services.AddScoped<IConnectionService, ConnectionService>();

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

    services.AddScoped<__IZookeeperRepository, __ZookeeperRepository>();
    services.AddScoped<IJobsRepository, JobsRepository>();
    services.AddScoped<ISpecialitiesRepository, SpecialitiesRepository>();
    services.AddScoped<__IZookeeperApiService, __ZookeeperApiService>();
    services.AddScoped<IJobsService, JobsService>();
    services.AddScoped<ISpecialitiesService, SpecialitiesService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddTransient<IJobsRequestReceivingService,JobsRequestReceivingService>();    
    services.AddTransient<ISpecialitiesRequestReceivingService, SpecialitiesRequestReceivingService>();
    services.AddTransient<IResponsesReceiverFromRabbitMq, ResponsesReceiverFromRabbitMq>();
    services.AddTransient<IApi, MicroZoo.ZookeepersApi.Apis.ZookeepersApi>();
    services.AddTransient<RequestHelper>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetAllJobsOfZookeeperConsumer>();
        x.AddConsumer<GetCurrentJobsOfZookeeperCustomer>();
        x.AddConsumer<GetJobsForTimeRangeConsumer>();
        x.AddConsumer<AddJobConsumer>();
        x.AddConsumer<UpdateJobConsumer>();
        x.AddConsumer<FinishJobConsumer>();

        x.AddConsumer<CheckZokeepersWithSpecialityAreExistConsumer>();
        x.AddConsumer<AddSpecialityConsumer>();
        x.AddConsumer<ChangeRelationBetweenZookeeperAndSpecialityConsumer>();
        x.AddConsumer<DeleteSpecialityConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("zookeepers-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<GetAllJobsOfZookeeperConsumer>(context);
                e.ConfigureConsumer<GetCurrentJobsOfZookeeperCustomer>(context);
                e.ConfigureConsumer<GetJobsForTimeRangeConsumer>(context);
                e.ConfigureConsumer<AddJobConsumer>(context);
                e.ConfigureConsumer<UpdateJobConsumer>(context);
                e.ConfigureConsumer<FinishJobConsumer>(context);

                e.ConfigureConsumer<CheckZokeepersWithSpecialityAreExistConsumer>(context);
                e.ConfigureConsumer<AddSpecialityConsumer>(context);
                e.ConfigureConsumer<ChangeRelationBetweenZookeeperAndSpecialityConsumer>(context);
                e.ConfigureConsumer<DeleteSpecialityConsumer>(context);
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

    //app.UseAuthentication();
    //app.UseAuthorization();

    app.MapControllers();
}