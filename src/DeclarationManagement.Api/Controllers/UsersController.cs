using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 用户Controller类。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// 用户服务字段。
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取数据。
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetList(CancellationToken cancellationToken)
    {
        var result = await _userService.GetListAsync(cancellationToken); // result：结果
        return Ok(ApiResponse<List<UserDto>>.Ok(result));
    }

    /// <summary>
    /// 创建数据。
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _userService.CreateAsync(request, cancellationToken); // id：ID
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    /// <summary>
    /// 更新数据。
    /// </summary>
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }

    /// <summary>
    /// 删除数据。
    /// </summary>
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(long id, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "删除成功"));
    }

    /// <summary>
    /// 重置数据。
    /// </summary>
    [HttpPost("{id:long}/reset-password")]
    public async Task<ActionResult<ApiResponse<string>>> ResetPassword(long id, CancellationToken cancellationToken)
    {
        await _userService.ResetPasswordAsync(id, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "重置成功"));
    }
}
