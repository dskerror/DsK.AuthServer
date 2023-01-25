using AutoMapper;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>();
            //.ForMember(dest => dest.ConfirmEmail,
            //           opt => opt.MapFrom(src => src.Email));
            CreateMap<User, UserCreateDto>();
        }
      
    }
}
