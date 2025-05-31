using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MicroZoo.IdentityApi.Services
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager,
                                                        TUser user,
                                                        string? password)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User can't be null.",
                    Code = "NullUser"
                });

            if (password.IsNullOrEmpty())
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Password can't be null or empty.",
                    Code = "PasswordIsNullOrEmpty"
                });

            var username = await manager.GetUserNameAsync(user);
            if (string.Equals(username, password, StringComparison.OrdinalIgnoreCase))
                return IdentityResult.Failed(new IdentityError
                { 
                    Description = "Username and Password can't be the same.",
                    Code = "SameUserPass"
                });

            if (password!.ToLower().Contains("password"))
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "The word \"Password\" is not allowed for the password.",
                    Code = "PasswordContainsPassword"
                });

            return IdentityResult.Success;
        }
    }
}
