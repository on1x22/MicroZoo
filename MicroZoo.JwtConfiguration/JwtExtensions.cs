using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MicroZoo.JwtConfiguration
{
    public static class JwtExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            /*var path = Directory.GetCurrentDirectory();

            var ddd = AppDomain.CurrentDomain.BaseDirectory;*/

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var jwtSettings = config.GetSection("JwtSettings");

            var iss = jwtSettings["validIssuer"];
            var aud = jwtSettings["validAudience"];
            var sk = jwtSettings["securityKey"];
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
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
        }

        public static string GetAccessTokenFromRequest(HttpRequest request)
        {
            var authorizationHeader = request.Headers["Authorization"]!.ToString();

            if (authorizationHeader == null)
                return default!;

            var accessToken = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
            return accessToken;
        }
    }
}
