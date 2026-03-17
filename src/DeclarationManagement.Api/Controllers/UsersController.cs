using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetList([FromQuery] UserQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _userService.GetListAsync(query, cancellationToken);
        return Ok(ApiResponse<List<UserDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _userService.CreateAsync(request, cancellationToken);
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(long id, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "删除成功"));
    }

    [HttpPost("{id:long}/reset-password")]
    public async Task<ActionResult<ApiResponse<string>>> ResetPassword(long id, CancellationToken cancellationToken)
    {
        await _userService.ResetPasswordAsync(id, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "重置成功"));
    }
}
