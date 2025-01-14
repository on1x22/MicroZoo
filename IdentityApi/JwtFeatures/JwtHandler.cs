using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MicroZoo.Infrastructure.Models.Users;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace MicroZoo.IdentityApi.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        public JwtHandler(IConfiguration configuration) 
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
        }

        public string CreateAccessToken(User user, IList<string> roles)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user, roles);
            var tokenOptions = GenerateTokenOption(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
        public ClaimsPrincipal GetPrincipalFromToken(string token) =>       
            ExtractPrincipalFromToken(token: token, validateLifetimeValue: true);

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) =>                    
            ExtractPrincipalFromToken(token: token, validateLifetimeValue : false);
        

        public DateTime GetRefreshTokenExpiryTimeSpanInDays()
        {
            var refreshTokenExpiryTimeSpanInDays = Convert.ToDouble(_jwtSettings["refreshTokenExpiryTimeSpanInDays"]);
            return DateTime.UtcNow.AddDays(refreshTokenExpiryTimeSpanInDays);
        }

        private ClaimsPrincipal ExtractPrincipalFromToken(string token, bool validateLifetimeValue)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!)),
                ValidateLifetime = validateLifetimeValue
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal;
            
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                        out securityToken);
            }
            catch (SecurityTokenExpiredException)
            {
                return default!;
            }
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException("Invalid token");

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(User user, IList<string> roles) 
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (var role in roles)            
                claims.Add(new Claim(ClaimTypes.Role, role));
            
            return claims;
        }

        private JwtSecurityToken GenerateTokenOption(SigningCredentials signingCredentials,
            List<Claim> claims) 
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials
                );

            return tokenOptions;
        }
    }
}
