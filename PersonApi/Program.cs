using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.CorrelationIdGenerator;
using MicroZoo.Infrastructure.MassTransit;
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
                new string[] {}
            }
        });
    });

    services.AddCorrelationIdGenerator();

    services.AddDbContext<PersonDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PersonapiDB"));
    });

    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IPersonsApiService, PersonsApiService>();
    services.AddScoped<IPersonsRequestReceivingService, PersonsRequestReceivingService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddScoped<IResponsesReceiverFromRabbitMq, ResponsesReceiverFromRabbitMq>();
    services.AddScoped<IConnectionService, ConnectionService>();
    services.AddScoped<IRabbitMqResponseErrorsHandler, RabbitMqResponseErrorsHandler>();
    services.AddTransient<IApi, PersonsApi>();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<GetPersonConsumer>();
        x.AddConsumer<AddPersonConsumer>();
        x.AddConsumer<UpdatePersonConsumer>();
        x.AddConsumer<DeletePersonConsumer>();

        x.AddConsumer<GetSubordinatePersonnelConsumer>();
        x.AddConsumer<ChangeManagerForSubordinatePersonnelConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
            cfg.ReceiveEndpoint("persons-queue", e =>
            {
                e.PrefetchCount = 20;
                e.UseMessageRetry(r => r.Interval(2, 100));

                e.ConfigureConsumer<GetPersonConsumer>(context);
                e.ConfigureConsumer<AddPersonConsumer>(context);                
                e.ConfigureConsumer<UpdatePersonConsumer>(context);
                e.ConfigureConsumer<DeletePersonConsumer>(context);

                e.ConfigureConsumer<GetSubordinatePersonnelConsumer>(context);
                e.ConfigureConsumer<ChangeManagerForSubordinatePersonnelConsumer>(context);
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

    app.UseAuthentication();

    app.MapControllers();
}