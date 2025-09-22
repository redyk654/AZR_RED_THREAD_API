// AZR_RED_THREAD_BLL/Profiles/ProjectProfile.cs
using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.CreateProjectDto;
using AZR_RED_THREAD_BLL.DTOs.ProjectDto;
using AZR_RED_THREAD_BLL.DTOs.UpdateProjectDto;
using AZR_RED_THREAD_DAL.Models.Project;

namespace AZR_RED_THREAD_BLL.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Project -> ProjectDto
            CreateMap<Project, ProjectDto>();

            // CreateProjectDto -> Project
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // UpdateProjectDto -> Project
            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}