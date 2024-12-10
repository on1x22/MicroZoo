

using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MicroZoo.JwtConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

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
    services.AddOcelot(builder.Configuration);
    /*services.AddJwtAuthentication();
    services.AddCors(opt =>
    {
        opt.AddPolicy("CORSPolicy", builder => builder.AllowAnyHeader()
        .AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true));
    });
    services.AddLogging(builder => builder.AddConsole());*/
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
    //app.UseRouting();
    //app.UseAuthentication();
    //app.UseAuthorization();
    //app.MapControllers();
    
    await app.UseOcelot();
}