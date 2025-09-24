using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.Access;
using AZR_RED_THREAD_DAL.Models.AccessAndPrivileges;

namespace AZR_RED_THREAD_BLL.Profiles
{
    public class AccessProfile : Profile
    {
        public AccessProfile()
        {
            CreateMap<Roles, RoleDto>().ReverseMap();
            CreateMap<Privilege, PrivilegeDto>().ReverseMap();
            CreateMap<CreateRoleDto, Roles>();
            CreateMap<UpdateRoleDto, Roles>();
        }
    }
}
