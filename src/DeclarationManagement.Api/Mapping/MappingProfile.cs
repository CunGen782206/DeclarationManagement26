using AutoMapper;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.Mapping;

/// <summary>
/// AutoMapper 映射配置。
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// 构造函数。
    /// </summary>
    public MappingProfile()
    {
        // 申报单：请求 DTO -> 实体
        CreateMap<SaveDeclarationRequestDto, Declaration>();

        // 申报单：实体 -> 详情 DTO
        CreateMap<Declaration, DeclarationDetailDto>()
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task != null ? src.Task.TaskName : string.Empty))
            .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.ApplicantUser != null ? src.ApplicantUser.FullName : string.Empty))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
            .ForMember(dest => dest.ProjectCategoryName, opt => opt.MapFrom(src => src.ProjectCategory != null ? src.ProjectCategory.Name : string.Empty));

        // 用户：实体 -> DTO
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));

        // 审核记录：实体 -> DTO
        CreateMap<DeclarationReviewRecord, ReviewRecordDto>();
    }
}
