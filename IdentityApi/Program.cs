
using MicroZoo.IdentityApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.IdentityApi.Models.Mappers;
using MicroZoo.Infrastructure.Models.Users;
using MicroZoo.Infrastructure.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.EmailService;

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

            services.AddAutoMapper(typeof(UserProfile));
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
