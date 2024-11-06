using Microsoft.AspNetCore.Identity;

namespace MicroZoo.Infrastructure.Models.Users
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public void Update(User userForUpdate)
        {
            FirstName = userForUpdate.FirstName;
            LastName = userForUpdate.LastName;
            UserName = userForUpdate.UserName;
            NormalizedUserName = userForUpdate.NormalizedUserName;
            Email = userForUpdate.Email;
            NormalizedEmail = userForUpdate.NormalizedEmail;
            EmailConfirmed = userForUpdate.EmailConfirmed;
            PasswordHash = userForUpdate.PasswordHash;
            SecurityStamp = userForUpdate.SecurityStamp;
            ConcurrencyStamp = userForUpdate.ConcurrencyStamp;
            PhoneNumber = userForUpdate.PhoneNumber;
            PhoneNumberConfirmed = userForUpdate.PhoneNumberConfirmed;
            TwoFactorEnabled = userForUpdate.TwoFactorEnabled;
            LockoutEnd = userForUpdate.LockoutEnd;
            LockoutEnabled = userForUpdate.LockoutEnabled;
            AccessFailedCount = userForUpdate.AccessFailedCount;
        }

        public UserWithRoles ConvertToUserWithRoles()
        {
            return new UserWithRoles
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                UserName = UserName,
                NormalizedUserName = NormalizedUserName,
                Email = Email,
                NormalizedEmail = NormalizedEmail,
                EmailConfirmed = EmailConfirmed,
                PasswordHash = PasswordHash,
                SecurityStamp = SecurityStamp,
                ConcurrencyStamp = ConcurrencyStamp,
                PhoneNumber = PhoneNumber,
                PhoneNumberConfirmed = PhoneNumberConfirmed,
                TwoFactorEnabled = TwoFactorEnabled,
                LockoutEnd = LockoutEnd,
                LockoutEnabled = LockoutEnabled,
                AccessFailedCount = AccessFailedCount
            };
        }
    }
}
