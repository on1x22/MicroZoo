using AutoFixture;
using MicroZoo.IdentityApi.Services;

namespace MicroZoo.IdentityApi.Tests.UnitTests
{
    public class EmailValidatorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name+tag@domain.co")]
        [InlineData("user@sub.domain.com")]
        [InlineData("firstname.lastname@example.com")]
        [InlineData("email@domain-one.com")]
        [InlineData("1234567890@example.com")]
        [InlineData("email@123.123.123.123")]
        public void Validate_ValidEmails_ShouldReturnTrue(string validEmail)
        {
            // Act
            var result = EmailValidator.Validate(validEmail);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("plaintext")]
        [InlineData("@missingusername.com")]
        [InlineData("username@.com")]
        [InlineData(".username@domain.com")]
        [InlineData("username@domain..com")]
        [InlineData("username@domain.com.")]
        [InlineData("user name@domain.com")]
        [InlineData("user@domain,com")]
        [InlineData("user@domain")]
        [InlineData("user@domain..com")]
        public void Validate_InvalidEmails_ShouldReturnFalse(string invalidEmail)
        {
            // Act
            var result = EmailValidator.Validate(invalidEmail);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Validate_ForEdgeCases_ShouldReturnFalse(string edgeCaseEmail)
        {
            // Act
            var result = EmailValidator.Validate(edgeCaseEmail);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_RandomString_ShouldReturnFalse()
        {
            // Arrange
            var randomString = _fixture.Create<string>();

            // Act
            var result = EmailValidator.Validate(randomString);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_WithoutPerformanceIssues_ShouldHandleLongStrings()
        {
            // Arrange
            var longString = new string('a', 10000) + "@domain.com";

            // Act
            var result = EmailValidator.Validate(longString);

            // Assert
            Assert.True(result);
        }
    }
}
