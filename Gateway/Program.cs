using MicroZoo.Infrastructure.CorrelationIdGenerator;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddOcelot(builder.Environment);

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

app.Run();


void RegisterServices(IServiceCollection services)
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddOcelot();  
    services.AddCorrelationIdGenerator();

    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.Enrich.FromLogContext()
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
}

async void Configure(WebApplication app)
{    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    //app.UseCors("CORSPolicy");
    app.UseHttpsRedirection();
    app.UseCorrelationIdMiddleware();
    //app.UseRouting();
    //app.UseAuthentication();
    //app.UseAuthorization();
    //app.MapControllers();

    await app.UseOcelot();
    
}