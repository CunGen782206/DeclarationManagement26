using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// DeclarationsController类。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeclarationsController : ControllerBase
{
    /// <summary>
    /// 申报业务服务。
    /// </summary>
    private readonly IDeclarationService _declarationService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public DeclarationsController(IDeclarationService declarationService)
    {
        _declarationService = declarationService;
    }

    /// <summary>
    /// 查询申报详情。
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<DeclarationDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetDetailAsync(id, User.GetUserId(), cancellationToken); // result：结果

        if (result == null)
        {
            return NotFound(ApiResponse<DeclarationDetailDto>.Fail("申报单不存在"));
        }

        return Ok(ApiResponse<DeclarationDetailDto>.Ok(result));
    }

    /// <summary>
    /// 新建申报草稿。
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] SaveDeclarationRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _declarationService.CreateAsync(User.GetUserId(), request, cancellationToken); // id：ID
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    /// <summary>
    /// 修改申报草稿或驳回后的申报。
    /// </summary>
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long id, [FromBody] SaveDeclarationRequestDto request, CancellationToken cancellationToken)
    {
        await _declarationService.UpdateAsync(id, User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }

    /// <summary>
    /// 提交申报。
    /// </summary>
    [HttpPost("submit")]
    public async Task<ActionResult<ApiResponse<string>>> Submit([FromBody] DeclarationSubmitRequestDto request, CancellationToken cancellationToken)
    {
        await _declarationService.SubmitAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "提交成功"));
    }

    /// <summary>
    /// 驳回后重提申报。
    /// </summary>
    [HttpPost("resubmit")]
    public async Task<ActionResult<ApiResponse<string>>> Resubmit([FromBody] DeclarationResubmitRequestDto request, CancellationToken cancellationToken)
    {
        await _declarationService.ResubmitAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "重提成功"));
    }

    /// <summary>
    /// 查询我的申报列表（分页）。
    /// </summary>
    [HttpGet("mine")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<DeclarationListItemDto>>>> Mine([FromQuery] DeclarationPageQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetMyDeclarationsAsync(User.GetUserId(), query, cancellationToken); // result：结果
        return Ok(ApiResponse<PagedResultDto<DeclarationListItemDto>>.Ok(result));
    }

    /// <summary>
    /// 上传申报附件。
    /// </summary>
    [HttpPost("{id:long}/attachments")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<long>>> Upload(long id, IFormFile file, CancellationToken cancellationToken)
    {
        var attachmentId = await _declarationService.UploadAttachmentAsync(id, User.GetUserId(), file, cancellationToken); // attachmentId：附件ID
        return Ok(ApiResponse<long>.Ok(attachmentId, "上传成功"));
    }

    /// <summary>
    /// 获取申报附件列表。
    /// </summary>
    [HttpGet("{id:long}/attachments")]
    public async Task<ActionResult<ApiResponse<List<AttachmentDto>>>> GetAttachments(long id, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetAttachmentsAsync(id, User.GetUserId(), cancellationToken); // result：结果
        return Ok(ApiResponse<List<AttachmentDto>>.Ok(result));
    }

    /// <summary>
    /// 下载单个附件。
    /// </summary>
    [HttpGet("attachments/{attachmentId:long}/download")]
    public async Task<IActionResult> DownloadAttachment(long attachmentId, CancellationToken cancellationToken)
    {
        var file = await _declarationService.DownloadAttachmentAsync(attachmentId, User.GetUserId(), cancellationToken); // file：文件
        return File(file.Content, file.ContentType, file.FileName);
    }
}
