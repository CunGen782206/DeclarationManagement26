using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// IReview服务接口。
/// </summary>
public interface IReviewService
{
    Task<PagedResultDto<PendingReviewItemDto>> GetPendingAsync(long reviewerUserId, PendingReviewQueryDto query, CancellationToken cancellationToken = default);
    Task ExecuteReviewAsync(long reviewerUserId, ReviewActionRequestDto request, CancellationToken cancellationToken = default);
    Task<List<ReviewRecordDto>> GetReviewRecordsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default);
}
