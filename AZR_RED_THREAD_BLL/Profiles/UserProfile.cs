using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.UserDto;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges.Users;

namespace AZR_RED_THREAD_BLL.Profiles
{
    /// <summary>
    /// Profil AutoMapper pour mapper User ↔ UserDto.
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map de base User -> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleLabel, opt => opt.MapFrom(src => src.Role != null ? src.Role.Label : null))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Si besoin un jour : UserDto -> User (pour update via DTO)
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()); // évite problème EF sur navigation
        }
    }
}
