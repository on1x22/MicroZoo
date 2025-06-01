using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.Infrastructure.Models.Users;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroZoo.IdentityApi.Tests.UnitTests.JwtFeatures
{
    public class JwtHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockJwtSettings;
        private readonly IJwtHandler _jwtHandler;

        public JwtHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockConfiguration = _fixture.Freeze<Mock<IConfiguration>>();
            _mockJwtSettings = _fixture.Freeze<Mock<IConfigurationSection>>();

            _mockConfiguration.Setup(x => x.GetSection("JwtSettings"))
                .Returns(_mockJwtSettings.Object);

            _mockJwtSettings.Setup(x => x["securityKey"]).Returns("super-secret-key-at-least-32-chars");
            _mockJwtSettings.Setup(x => x["validIssuer"]).Returns("test-issuer");
            _mockJwtSettings.Setup(x => x["validAudience"]).Returns("test-audience");
            _mockJwtSettings.Setup(x => x["expiryInMinutes"]).Returns("30");            

            _jwtHandler = new JwtHandler(_mockConfiguration.Object);
        }

        [Fact]
        public void CreateAccessToken_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "test_user" };
            var roles = new List<string> { "role_1" };

            // Act
            Action act = () => _jwtHandler.CreateAccessToken(null!, roles);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'Cannot get UserName " +
                "because user is null')", exception.Message);            
        }

        [Fact]
        public void CreateAccessToken_WithNullRoles_ThrowsArgumentNullException()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "test_user" };

            // Act
            Action act = () => _jwtHandler.CreateAccessToken(user, null!);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'User roles is null')", 
                exception.Message);
        }

        [Fact]
        public void CreateAccessToken_WithEmptyRoles_WorkWithEmptyRoles()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "test_user" };

            // Act
            var token = _jwtHandler.CreateAccessToken(user, new List<string>());
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Assert
            Assert.DoesNotContain(jwtToken.Claims, c => c.Type == ClaimTypes.Role);
        }

        [Fact]
        public void CreateAccessToken_GeneratesValidSignedToken()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "test_user" };
            var roles = new List<string> { "role_1" };

            // Act
            var token = _jwtHandler.CreateAccessToken(user, roles);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "test-issuer",
                ValidAudience = "test-audience",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("super-secret-key-at-least-32-chars"))
            };

            var principal = handler.ValidateToken(token, validationParameters, out _);
            Assert.NotNull(principal);
        }

        [Fact]
        public void CreateAccessToken_TokenExpirationIsCorrect()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "test_user" };
            var roles = new List<string> { "role_1" };

            // Act
            var token = _jwtHandler.CreateAccessToken(user, roles);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Assert
            var expectedExpiration = DateTime.UtcNow.AddMinutes(30);
            Assert.True(jwtToken.ValidTo >= expectedExpiration.AddMinutes(-1));
            Assert.True(jwtToken.ValidTo <= expectedExpiration.AddMinutes(1));
        }

        [Fact]
        public void CreateAccessToken_ReturnsValidAccessToken()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(x => x.Id, "1")
                .With(x => x.UserName, "test_user")
                .With(x => x.Email, "test_user@test.com")
                .Create();
            var mockRoles = new List<string> { "role_1", "role_2" };

            // Act
            var resultToken = _jwtHandler.CreateAccessToken(user, mockRoles);

            // Assert
            Assert.NotNull(resultToken);
            Assert.NotEmpty(resultToken);

            // Checking token structure
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(resultToken);

            Assert.Equal("test-issuer", jwtToken.Issuer);
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Name && c.Value == "test_user");
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "role_1");
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "role_2");
        }
    }
}
