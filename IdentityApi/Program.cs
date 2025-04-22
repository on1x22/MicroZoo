
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MicroZoo.EmailService;
using MicroZoo.IdentityApi.Consumers;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Policies;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.Infrastructure.Models.Users;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Text;

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
                    .ReadFrom.Configuration(context.Configuration);
            });

            services.AddDbContext<IdentityApiDbContext>(opts =>
                opts.UseNpgsql(builder.Configuration.GetConnectionString("IdentityApi")));

            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<IdentityApiDbContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<CustomPasswordValidator<User>>();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["validIssuer"],
                        ValidAudience = jwtSettings["validAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(jwtSettings["securityKey"]!))
                    };
                });

            services.AddAuthorization(opt => PoliciesConfiguration.AddAuthorizationOptions(opt));
            services.AddScoped<IAuthorizationHandler,AllowedRequirementsHandler>();

            services.AddSingleton<JwtHandler>();

            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
                        
            services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
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
                        new string[]{ }
                    }
                });
            });

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();      
            
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<IRequirementsRepository, RequirementsRepository>();
            services.AddScoped<IRequirementsService, RequirementsService>();

            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<IUserRolesService, UserRolesService>();

            services.AddScoped<IRoleRequirementsRepository, RoleRequirementsRepository>();
            services.AddScoped<IRoleRequirementsService, RoleRequirementsService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CheckAccessConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
                    cfg.ReceiveEndpoint("identity-queue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        e.ConfigureConsumer<CheckAccessConsumer>(context);
                    });
                });
            });
        }

        static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
        }
    }
}
