using AutoMapper;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using DeclarationManagement.Api.Repositories;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 申报业务服务实现：Controller 仅调用该服务，不直接处理复杂逻辑。
/// </summary>
public class DeclarationService : IDeclarationService
{
    private readonly IRepository<Declaration> _declarationRepository;
    private readonly IMapper _mapper;

    public DeclarationService(IRepository<Declaration> declarationRepository, IMapper mapper)
    {
        _declarationRepository = declarationRepository;
        _mapper = mapper;
    }

    public async Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, CancellationToken cancellationToken = default)
    {
        var declaration = await _declarationRepository.GetByIdAsync(declarationId, cancellationToken);
        return declaration == null ? null : _mapper.Map<DeclarationDetailDto>(declaration);
    }

    public async Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Declaration>(request);
        entity.ApplicantUserId = applicantUserId;
        entity.CurrentStatus = DeclarationStatus.Draft;
        entity.CurrentNode = DeclarationNode.Declaration;
        entity.CreatedAt = DateTime.UtcNow;

        await _declarationRepository.AddAsync(entity, cancellationToken);
        await _declarationRepository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default)
    {
        var list = await _declarationRepository.GetListAsync(x => x.ApplicantUserId == applicantUserId, cancellationToken);

        var items = list
            .OrderByDescending(x => x.SubmittedAt ?? x.CreatedAt)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new DeclarationListItemDto
            {
                Id = x.Id,
                ProjectName = x.ProjectName,
                CurrentStatus = x.CurrentStatus,
                SubmittedAt = x.SubmittedAt
            })
            .ToList();

        return new PagedResultDto<DeclarationListItemDto>
        {
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            TotalCount = list.Count,
            Items = items
        };
    }
}
