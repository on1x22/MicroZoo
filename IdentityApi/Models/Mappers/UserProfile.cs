using AutoMapper;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Models.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserForRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
