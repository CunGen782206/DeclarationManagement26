using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// IDeclaration服务接口。
/// </summary>
public interface IDeclarationService
{
    Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default);
    Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateAsync(long declarationId, long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default);
    Task<long> SubmitAsync(long applicantUserId, DeclarationSubmitRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default);
    Task<long> UploadAttachmentAsync(long declarationId, long uploaderId, IFormFile file, CancellationToken cancellationToken = default);
    Task<long> UploadTemporaryAttachmentAsync(string tempAttachmentKey, long uploaderId, IFormFile file, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string FileName, string ContentType)> DownloadAttachmentAsync(long attachmentId, long currentUserId, CancellationToken cancellationToken = default);
    Task<List<AttachmentDto>> GetAttachmentsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default);
    Task<List<AttachmentDto>> GetTemporaryAttachmentsAsync(string tempAttachmentKey, long currentUserId, CancellationToken cancellationToken = default);
    Task ClearTemporaryAttachmentsAsync(string tempAttachmentKey, long currentUserId, CancellationToken cancellationToken = default);
}
