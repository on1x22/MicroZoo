using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.Apis;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.PersonsApi.Repository;

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
    services.AddDbContext<PersonDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PersonapiDB"));
    });

    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddTransient<IApi, PersonApi>();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PersonDbContext>();
        db.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();
}