using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 认证控制器。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return Ok(ApiResponse<LoginResponseDto>.Ok(result));
    }

    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<CurrentUserDto>>> Me(CancellationToken cancellationToken)
    {
        // TODO: 接入 JWT 后从 Claim 读取 userId。
        const long currentUserId = 1;
        var result = await _authService.GetCurrentUserAsync(currentUserId, cancellationToken);
        return Ok(ApiResponse<CurrentUserDto>.Ok(result));
    }
}
