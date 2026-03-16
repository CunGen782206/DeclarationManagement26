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
        // 基础状态：先创建为“草稿”，后续可在工作流服务中转“提交/待预审”。
        var entity = _mapper.Map<Declaration>(request);
        entity.ApplicantUserId = applicantUserId;
        entity.CurrentStatus = DeclarationStatus.Draft;
        entity.CurrentNode = DeclarationNode.Declaration;
        entity.CreatedAt = DateTime.UtcNow;

        await _declarationRepository.AddAsync(entity, cancellationToken);
        await _declarationRepository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
