using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace MicroZoo.IdentityApi.Services
{
    public static class EmailValidator
    {
        public static bool Validate(string email)
        {
            if (email.IsNullOrEmpty())
                return false;

            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        
            return validateEmailRegex.IsMatch(email);
        }
    }
}
