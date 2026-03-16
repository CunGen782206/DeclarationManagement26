using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var result = await _reviewService.GetPendingAsync(User.GetUserId(), query, cancellationToken);
        return Ok(ApiResponse<PagedResultDto<PendingReviewItemDto>>.Ok(result));
    }

    [HttpPost("action")]
    public async Task<ActionResult<ApiResponse<string>>> Execute([FromBody] ReviewActionRequestDto request, CancellationToken cancellationToken)
    {
        await _reviewService.ExecuteReviewAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "审核完成"));
    }

    [HttpGet("{declarationId:long}/records")]
    public async Task<ActionResult<ApiResponse<List<ReviewRecordDto>>>> Records(long declarationId, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetReviewRecordsAsync(declarationId, User.GetUserId(), cancellationToken);
        return Ok(ApiResponse<List<ReviewRecordDto>>.Ok(result));
    }
}
