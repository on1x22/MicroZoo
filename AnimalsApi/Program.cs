using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.Apis;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.AnimalsApi.Repository;

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
    services.AddEndpointsApiExplorer();
    
    services.AddDbContext<AnimalDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AnimalsAPI"));
    });

    services.AddScoped<IAnimalRepository, AnimalRepository>();
    services.AddTransient<IApi, AnimalApi>();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnimalDbContext>();
        db.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();
}