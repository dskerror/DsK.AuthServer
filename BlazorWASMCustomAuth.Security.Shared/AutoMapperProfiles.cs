using AutoMapper;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;

namespace BlazorWASMCustomAuth.Security.Shared;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //ActionDtos
        CreateMap<AuthenticationProviderCreateDto, AuthenticationProvider>().ReverseMap();
        CreateMap<AuthenticationProviderUpdateDto, AuthenticationProvider>().ReverseMap();
        CreateMap<PermissionCreateDto, Permission>().ReverseMap();
        CreateMap<PermissionUpdateDto, Permission>().ReverseMap();
        CreateMap<RolePermissionChangeDto, RolePermission>().ReverseMap();        
        CreateMap<RoleCreateDto, Role>().ReverseMap();
        CreateMap<RoleUpdateDto, Role>().ReverseMap();
        CreateMap<UserPermissionCreateDto, UserPermission>().ReverseMap();
        CreateMap<UserCreateDto, User>().ReverseMap();
        
        //.ForMember(dest => dest.ConfirmEmail,
        //           opt => opt.MapFrom(src => src.Email));
        //CreateMap<User, UserCreateDto>();

        CreateMap<User, User>().ReverseMap().ForMember(dest => dest.Username, act => act.Ignore());

        //ModelDtos
        CreateMap<AuthenticationProviderDto, AuthenticationProvider>().ReverseMap();
        CreateMap<PermissionDto, Permission>().ReverseMap();
        CreateMap<RolePermissionGridDto, Permission>().ReverseMap();
        CreateMap<RoleDto, Role>().ReverseMap();
        CreateMap<RolePermissionDto, RolePermission>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();        
        CreateMap<UserLogDto, UserLog>().ReverseMap();
        CreateMap<UserPasswordDto, UserPassword>().ReverseMap();
        CreateMap<UserPermissionDto, UserPermission>().ReverseMap();
        CreateMap<UserRoleDto, UserRole>().ReverseMap();
        CreateMap<UserTokenDto, UserToken>().ReverseMap();
    }  
}
