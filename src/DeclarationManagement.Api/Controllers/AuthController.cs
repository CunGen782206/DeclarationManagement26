using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// AuthController 类。
/// </summary>
public class AuthController : ControllerBase
{
    /// <summary>
    /// _authService 字段。
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    /// <summary>
    /// 登录处理。
    /// </summary>
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return Ok(ApiResponse<LoginResponseDto>.Ok(result));
    }

    [HttpGet("me")]
    [Authorize]
    /// <summary>
    /// Me 方法。
    /// </summary>
    public async Task<ActionResult<ApiResponse<CurrentUserDto>>> Me(CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        var result = await _authService.GetCurrentUserAsync(currentUserId, cancellationToken);
        return Ok(ApiResponse<CurrentUserDto>.Ok(result));
    }

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
