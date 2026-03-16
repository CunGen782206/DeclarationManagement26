using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 审核控制器（预审/初审）。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<PendingReviewItemDto>>>> GetPending([FromQuery] PendingReviewQueryDto query, CancellationToken cancellationToken)
    {
        const long currentUserId = 1;
        var result = await _reviewService.GetPendingAsync(currentUserId, query, cancellationToken);
        return Ok(ApiResponse<PagedResultDto<PendingReviewItemDto>>.Ok(result));
    }

    [HttpPost("action")]
    public async Task<ActionResult<ApiResponse<string>>> Execute([FromBody] ReviewActionRequestDto request, CancellationToken cancellationToken)
    {
        const long currentUserId = 1;
        await _reviewService.ExecuteReviewAsync(currentUserId, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "审核完成"));
    }
}
