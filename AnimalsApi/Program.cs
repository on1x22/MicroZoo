using MassTransit;
using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.Apis;
using MicroZoo.AnimalsApi.Consumers;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

var apis = app.Services.GetServices<IApi>();
foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found");
    api.Register(app);
}

app.Run();


void RegisterServices(IServiceCollection services)
{
    services.AddLogging(builder => builder.AddConsole());
    services.AddAuthentication("Bearer").AddJwtBearer();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    services.AddDbContext<AnimalDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AnimalsAPI"));
    });

    services.AddScoped<IAnimalRepository, AnimalRepository>();
    services.AddScoped<IAnimalsApiService, AnimalsApiService>();
    services.AddScoped<IResponsesReceiverFromRabbitMq, ResponsesReceiverFromRabbitMq>();
    services.AddScoped<IAnimalsRequestReceivingService, AnimalsRequestReceivingService>();
    services.AddScoped<IAnimalTypesRequestReceivingService, AnimalTypesRequestReceivingService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddScoped<IConnectionService, ConnectionService>();
    services.AddScoped<IRabbitMqResponseErrorsHandler, RabbitMqResponseErrorsHandler>();
    services.AddTransient<IApi, MicroZoo.AnimalsApi.Apis.AnimalsApi>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetAllAnimalsConsumer>();
        x.AddConsumer<GetAnimalConsumer>();
        x.AddConsumer<AddAnimalConsumer>();
        x.AddConsumer<UpdateAnimalConsumer>();
        x.AddConsumer<DeleteAnimalConsumer>();
        x.AddConsumer<GetAnimalsByTypesConsumer>();

        x.AddConsumer<GetAllAnimalTypesConsumer>();
        x.AddConsumer<GetAnimalTypeConsumer>();
        x.AddConsumer<AddAnimalTypeConsumer>();
        x.AddConsumer<UpdateAnimalTypeConsumer>();
        x.AddConsumer<DeleteAnimalTypeConsumer>();
        x.AddConsumer<GetAnimalTypesByIdsConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("animals-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<GetAllAnimalsConsumer>(context);
                e.ConfigureConsumer<GetAnimalConsumer>(context);
                e.ConfigureConsumer<AddAnimalConsumer>(context);
                e.ConfigureConsumer<UpdateAnimalConsumer>(context);
                e.ConfigureConsumer<DeleteAnimalConsumer>(context);
                e.ConfigureConsumer<GetAnimalsByTypesConsumer>(context);

                e.ConfigureConsumer<GetAllAnimalTypesConsumer>(context);
                e.ConfigureConsumer<GetAnimalTypeConsumer>(context);
                e.ConfigureConsumer<AddAnimalTypeConsumer>(context);
                e.ConfigureConsumer<UpdateAnimalTypeConsumer>(context);
                e.ConfigureConsumer<DeleteAnimalTypeConsumer>(context);
                e.ConfigureConsumer<GetAnimalTypesByIdsConsumer>(context);
            });
            cfg.ConfigureEndpoints(context);
        });
    });
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnimalDbContext>();
        db.Database.EnsureCreated();

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.MapControllers();
}