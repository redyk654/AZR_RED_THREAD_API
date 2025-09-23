using AutoMapper;
using AZR_RED_THREAD_BLL.DTOs.TaskDto;
using TaskEntity = AZR_RED_THREAD_DAL.Models.Task.Task;

namespace AZR_RED_THREAD_BLL.Profiles
{
    /// <summary>
    /// Profile AutoMapper pour Task: entité <-> DTOs
    /// </summary>
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // Entité -> DTO (lecture)
            CreateMap<TaskEntity, TaskDto>()
                // Si Project navigation existe, mappez son nom
                .ForMember(dest => dest.ProjectName,
                    opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : null));

            // Create / Update DTO -> Entité
            CreateMap<CreateTaskDto, TaskEntity>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // par défaut active

            CreateMap<UpdateTaskDto, TaskEntity>();
        }
    }
}
