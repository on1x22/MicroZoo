using System.Text.RegularExpressions;

namespace MicroZoo.IdentityApi.Services
{
    public static class EmailValidator
    {
        public static bool Validate(string email)
        {
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        
            return validateEmailRegex.IsMatch(email);
        }
    }
}
