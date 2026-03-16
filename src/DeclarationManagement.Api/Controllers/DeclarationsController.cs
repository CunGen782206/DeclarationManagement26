using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 项目申报控制器：仅负责请求接收与响应返回。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DeclarationsController : ControllerBase
{
    private readonly IDeclarationService _declarationService;

    public DeclarationsController(IDeclarationService declarationService)
    {
        _declarationService = declarationService;
    }

    /// <summary>
    /// 获取申报详情。
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<DeclarationDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetDetailAsync(id, cancellationToken);

        if (result == null)
        {
            return NotFound(ApiResponse<DeclarationDetailDto>.Fail("申报单不存在"));
        }

        return Ok(ApiResponse<DeclarationDetailDto>.Ok(result));
    }

    /// <summary>
    /// 新建申报。
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] SaveDeclarationRequestDto request, CancellationToken cancellationToken)
    {
        // 示例中先写死当前用户ID；接入JWT后应从 Claim 读取。
        const long currentUserId = 1;

        var id = await _declarationService.CreateAsync(currentUserId, request, cancellationToken);
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    /// <summary>
    /// 获取我的申报分页列表。
    /// </summary>
    [HttpGet("mine")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<DeclarationListItemDto>>>> Mine([FromQuery] DeclarationPageQueryDto query, CancellationToken cancellationToken)
    {
        const long currentUserId = 1;
        var result = await _declarationService.GetMyDeclarationsAsync(currentUserId, query, cancellationToken);
        return Ok(ApiResponse<PagedResultDto<DeclarationListItemDto>>.Ok(result));
    }
}
