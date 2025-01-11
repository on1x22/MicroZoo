using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Models.Mappers
{
    public static class UserUpdater
    {
        public static User Update(User from, User to)
        {
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.UserName = from.UserName;
            to.NormalizedUserName = from.NormalizedUserName;
            to.Email = from.Email;
            to.NormalizedEmail = from.NormalizedEmail;
            to.EmailConfirmed = from.EmailConfirmed;
            to.PasswordHash = from.PasswordHash;
            to.SecurityStamp = from.SecurityStamp;
            to.ConcurrencyStamp = from.ConcurrencyStamp;
            to.PhoneNumber = from.PhoneNumber;
            to.PhoneNumberConfirmed = from.PhoneNumberConfirmed;
            to.TwoFactorEnabled = from.TwoFactorEnabled;
            to.LockoutEnd = from.LockoutEnd;
            to.LockoutEnabled = from.LockoutEnabled;
            to.AccessFailedCount = from.AccessFailedCount;

            return to;
        }

        public static User UpdateFromUserForUpdateDto(UserForUpdateDto from, User to)
        {
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.UserName = from.Username;
            to.Email = from.Email;
            to.PhoneNumber = from.PhoneNumber;

            return to;
        }

        public static User ConvertFromUserForRegistrationDto(UserForRegistrationDto from)
        {
            return new User
            {
                FirstName = from.FirstName,
                LastName = from.LastName,
                Email = from.Email,
                UserName = from.Email
            };
        }

        public static UserWithRoles ConvertToUserWithRoles(User from)
        {
            return new UserWithRoles
            {
                Id = from.Id,
                FirstName = from.FirstName,
                LastName = from.LastName,
                UserName = from.UserName,
                NormalizedUserName = from.NormalizedUserName,
                Email = from.Email,
                NormalizedEmail = from.NormalizedEmail,
                EmailConfirmed = from.EmailConfirmed,
                PasswordHash = from.PasswordHash,
                SecurityStamp = from.SecurityStamp,
                ConcurrencyStamp = from.ConcurrencyStamp,
                PhoneNumber = from.PhoneNumber,
                PhoneNumberConfirmed = from.PhoneNumberConfirmed,
                TwoFactorEnabled = from.TwoFactorEnabled,
                LockoutEnd = from.LockoutEnd,
                LockoutEnabled = from.LockoutEnabled,
                AccessFailedCount = from.AccessFailedCount
            };
        }
    }
}
