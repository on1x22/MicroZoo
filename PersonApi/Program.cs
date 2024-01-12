using MassTransit;
using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.Apis;
using MicroZoo.PersonsApi.Consumers;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.PersonsApi.Repository;
using MicroZoo.PersonsApi.Services;
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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    services.AddDbContext<PersonDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PersonapiDB"));
    });

    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IPersonsApiService, PersonsApiService>();
    services.AddTransient<IApi, PersonApi>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetPersonConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("persons-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<GetPersonConsumer>(context);               
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
        var db = scope.ServiceProvider.GetRequiredService<PersonDbContext>();
        db.Database.EnsureCreated();

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
}