using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// ReviewsController类。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    /// <summary>
    /// 审核服务字段。
    /// </summary>
    private readonly IReviewService _reviewService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    /// <summary>
    /// 获取数据。
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<PendingReviewItemDto>>>> GetPending([FromQuery] PendingReviewQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetPendingAsync(User.GetUserId(), query, cancellationToken); // result：结果
        return Ok(ApiResponse<PagedResultDto<PendingReviewItemDto>>.Ok(result));
    }

    /// <summary>
    /// 执行处理。
    /// </summary>
    [HttpPost("action")]
    public async Task<ActionResult<ApiResponse<string>>> Execute([FromBody] ReviewActionRequestDto request, CancellationToken cancellationToken)
    {
        await _reviewService.ExecuteReviewAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "审核完成"));
    }

    /// <summary>
    /// Records 方法。
    /// </summary>
    [HttpGet("{declarationId:long}/records")]
    public async Task<ActionResult<ApiResponse<List<ReviewRecordDto>>>> Records(long declarationId, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetReviewRecordsAsync(declarationId, User.GetUserId(), cancellationToken); // result：结果
        return Ok(ApiResponse<List<ReviewRecordDto>>.Ok(result));
    }
}
