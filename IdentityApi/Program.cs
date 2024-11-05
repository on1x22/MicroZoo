
using MicroZoo.IdentityApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;

namespace MicroZoo.IdentityApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            RegisterServices(builder.Services, builder);            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            Configure(app);            

            app.Run();
        }

        static void RegisterServices(IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<IdentityApiDbContext>(opts =>
                opts.UseNpgsql(builder.Configuration.GetConnectionString("IdentityApi")));

            services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();      
            
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<IRequirementsRepository, RequirementsRepository>();
            services.AddScoped<IRequirementsService, RequirementsService>();
        }

        static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
        }
    }
}
