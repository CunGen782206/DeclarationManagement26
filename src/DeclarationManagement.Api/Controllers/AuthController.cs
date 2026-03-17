using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// AuthController类。
/// </summary>
[ApiController]
[Route("api/[controller]")]
/// <summary>
/// AuthController类。
/// </summary>
public class AuthController : ControllerBase
{
    /// <summary>
    /// auth服务字段。
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 登录处理。
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    /// <summary>
    /// 登录处理。
    /// </summary>
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken); // result：结果
        return Ok(ApiResponse<LoginResponseDto>.Ok(result));
    }

    /// <summary>
    /// Me 方法。
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    /// <summary>
    /// Me 方法。
    /// </summary>
    public async Task<ActionResult<ApiResponse<CurrentUserDto>>> Me(CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId(); // currentUserId：当前用户ID
        var result = await _authService.GetCurrentUserAsync(currentUserId, cancellationToken); // result：结果
        return Ok(ApiResponse<CurrentUserDto>.Ok(result));
    }

    /// <summary>
    /// 变更处理。
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    /// <summary>
    /// 变更处理。
    /// </summary>
    public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "密码修改成功"));
    }
}
