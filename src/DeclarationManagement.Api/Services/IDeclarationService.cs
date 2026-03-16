using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 申报业务服务接口：承载申报相关业务逻辑。
/// </summary>
public interface IDeclarationService
{
    Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, CancellationToken cancellationToken = default);
    Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default);
}
