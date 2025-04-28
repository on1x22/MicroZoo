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
using Microsoft.OpenApi.Models;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.CorrelationIdGenerator;
using MicroZoo.ZookeepersApi;

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
    services.AddScoped<IConnectionService, ConnectionService>();

    services.AddHttpClient();
    services.AddLogging(builder => builder.AddConsole());
    services.AddAuthentication("Bearer").AddJwtBearer();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(opt =>
    {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        opt.IncludeXmlComments(xmlPath);

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });

    services.AddCorrelationIdGenerator();
    services.AddHttpContextAccessor();

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
    services.AddScoped<IRabbitMqResponseErrorsHandler, RabbitMqResponseErrorsHandler>();
    services.AddTransient<IJobsRequestReceivingService,JobsRequestReceivingService>();    
    services.AddTransient<ISpecialitiesRequestReceivingService, SpecialitiesRequestReceivingService>();
    services.AddTransient<IResponsesReceiverFromRabbitMq, ResponsesReceiverFromRabbitMq>();    
    services.AddTransient<IApi, MicroZoo.ZookeepersApi.Apis.ZookeepersApi>();
    services.AddTransient<RequestHelper>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetAllJobsOfZookeeperConsumer>();
        x.AddConsumer<GetCurrentJobsOfZookeeperConsumer>();
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
            cfg.ConfigureSend(sendCfg =>
            {
                sendCfg.UseFilter(new CorrelationIdSendFilter<HttpContext>(
                    context.GetRequiredService<IHttpContextAccessor>()));
            });
            //cfg.UseSendFilter(typeof(CorrelationIdSendFilter<>), context);            
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("zookeepers-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<GetAllJobsOfZookeeperConsumer>(context);
                e.ConfigureConsumer<GetCurrentJobsOfZookeeperConsumer>(context);
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

    app.UseCorrelationIdMiddleware();

    app.UseAuthentication();

    app.MapControllers();
}