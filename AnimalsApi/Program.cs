using AnimalsApi.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.Apis;
using MicroZoo.AnimalsApi.Consumers;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using System.Reflection;
using System.Xml.Linq;

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
    services.AddTransient<IApi, AnimalApi>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetAllAnimalsConsumer>();
        x.AddConsumer<GetAnimalConsumer>();
        x.AddConsumer<AddAnimalConsumer>();
        x.AddConsumer<UpdateAnimalConsumer>();
        x.AddConsumer<DeleteAnimalConsumer>();

        x.AddConsumer<GetAllAnimalTypesConsumer>();
        x.AddConsumer<GetAnimalTypeConsumer>();
        x.AddConsumer<AddAnimalTypeConsumer>();
        x.AddConsumer<UpdateAnimalTypeConsumer>();
        x.AddConsumer<DeleteAnimalTypeConsumer>();

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

                e.ConfigureConsumer<GetAllAnimalTypesConsumer>(context);
                e.ConfigureConsumer<GetAnimalTypeConsumer>(context);
                e.ConfigureConsumer<AddAnimalTypeConsumer>(context);
                e.ConfigureConsumer<UpdateAnimalTypeConsumer>(context);
                e.ConfigureConsumer<DeleteAnimalTypeConsumer>(context);
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
    app.MapControllers();
}