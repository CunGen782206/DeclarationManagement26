using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// IDeclarationService 接口。
/// </summary>
public interface IDeclarationService
{
    Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default);
    Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateAsync(long declarationId, long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default);
    Task SubmitAsync(long applicantUserId, DeclarationSubmitRequestDto request, CancellationToken cancellationToken = default);
    Task ResubmitAsync(long applicantUserId, DeclarationResubmitRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default);
    Task<long> UploadAttachmentAsync(long declarationId, long uploaderId, IFormFile file, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string FileName, string ContentType)> DownloadAttachmentAsync(long attachmentId, long currentUserId, CancellationToken cancellationToken = default);
    Task<List<AttachmentDto>> GetAttachmentsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default);
}
