using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
/// <summary>
/// ReviewsController 类。
/// </summary>
public class ReviewsController : ControllerBase
{
    /// <summary>
    /// _reviewService 字段。
    /// </summary>
    private readonly IReviewService _reviewService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("pending")]
    /// <summary>
    /// 获取数据。
    /// </summary>
    public async Task<ActionResult<ApiResponse<PagedResultDto<PendingReviewItemDto>>>> GetPending([FromQuery] PendingReviewQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetPendingAsync(User.GetUserId(), query, cancellationToken);
        return Ok(ApiResponse<PagedResultDto<PendingReviewItemDto>>.Ok(result));
    }

    [HttpPost("action")]
    /// <summary>
    /// 执行处理。
    /// </summary>
    public async Task<ActionResult<ApiResponse<string>>> Execute([FromBody] ReviewActionRequestDto request, CancellationToken cancellationToken)
    {
        await _reviewService.ExecuteReviewAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "审核完成"));
    }

    [HttpGet("{declarationId:long}/records")]
    /// <summary>
    /// Records 方法。
    /// </summary>
    public async Task<ActionResult<ApiResponse<List<ReviewRecordDto>>>> Records(long declarationId, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetReviewRecordsAsync(declarationId, User.GetUserId(), cancellationToken);
        return Ok(ApiResponse<List<ReviewRecordDto>>.Ok(result));
    }
}
