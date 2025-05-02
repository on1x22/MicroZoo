using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.CorrelationIdGenerator;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.MiddlewareFilters;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.ZookeepersApi;
using MicroZoo.ZookeepersApi.Apis;
using MicroZoo.ZookeepersApi.Consumers.Jobs;
using MicroZoo.ZookeepersApi.Consumers.Specialities;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

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
    services.AddHttpContextAccessor();
    services.AddCorrelationIdGenerator(); 
    
    builder.Host.UseSerilog((context, config) =>
    {
        config.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]!))
                {
                    IndexFormat = $"{context.Configuration["ApplicationName"]}-log-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .Enrich.WithCorrelationIdHeader("X-Correlation-Id")
            .ReadFrom.Configuration(context.Configuration);
    });

    services.AddHttpClient();
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

    services.AddDbContext<ZookeeperDBContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ZookeepersAPI"));
    });

    services.AddScoped<IConnectionService, ConnectionService>();
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
                sendCfg.UseFilter(new CorrelationIdSendFilter<CheckAccessRequest>(
                    context.GetRequiredService<IHttpContextAccessor>(),
                    context.GetRequiredService<ILogger<CorrelationIdSendFilter<CheckAccessRequest>>>()));
            });

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