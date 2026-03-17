using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeclarationsController : ControllerBase
{
    private readonly IDeclarationService _declarationService;

    public DeclarationsController(IDeclarationService declarationService)
    {
        _declarationService = declarationService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<DeclarationDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetDetailAsync(id, User.GetUserId(), cancellationToken);
        if (result == null)
        {
            return NotFound(ApiResponse<DeclarationDetailDto>.Fail("申报单不存在"));
        }

        return Ok(ApiResponse<DeclarationDetailDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] SaveDeclarationRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _declarationService.CreateAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long id, [FromBody] SaveDeclarationRequestDto request, CancellationToken cancellationToken)
    {
        await _declarationService.UpdateAsync(id, User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }

    [HttpPost("submit")]
    public async Task<ActionResult<ApiResponse<long>>> Submit([FromBody] DeclarationSubmitRequestDto request, CancellationToken cancellationToken)
    {
        var declarationId = await _declarationService.SubmitAsync(User.GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<long>.Ok(declarationId, "提交成功"));
    }

    [HttpGet("mine")]
    public async Task<ActionResult<ApiResponse<PagedResultDto<DeclarationListItemDto>>>> Mine([FromQuery] DeclarationPageQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetMyDeclarationsAsync(User.GetUserId(), query, cancellationToken);
        return Ok(ApiResponse<PagedResultDto<DeclarationListItemDto>>.Ok(result));
    }

    [HttpPost("{id:long}/attachments")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<long>>> Upload(long id, IFormFile file, CancellationToken cancellationToken)
    {
        var attachmentId = await _declarationService.UploadAttachmentAsync(id, User.GetUserId(), file, cancellationToken);
        return Ok(ApiResponse<long>.Ok(attachmentId, "上传成功"));
    }

    [HttpPost("temp-attachments")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<long>>> UploadTemporary([FromQuery] TemporaryAttachmentQueryDto query, IFormFile file, CancellationToken cancellationToken)
    {
        var attachmentId = await _declarationService.UploadTemporaryAttachmentAsync(query.TempAttachmentKey, User.GetUserId(), file, cancellationToken);
        return Ok(ApiResponse<long>.Ok(attachmentId, "上传成功"));
    }

    [HttpGet("{id:long}/attachments")]
    public async Task<ActionResult<ApiResponse<List<AttachmentDto>>>> GetAttachments(long id, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetAttachmentsAsync(id, User.GetUserId(), cancellationToken);
        return Ok(ApiResponse<List<AttachmentDto>>.Ok(result));
    }

    [HttpGet("temp-attachments")]
    public async Task<ActionResult<ApiResponse<List<AttachmentDto>>>> GetTemporaryAttachments([FromQuery] TemporaryAttachmentQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _declarationService.GetTemporaryAttachmentsAsync(query.TempAttachmentKey, User.GetUserId(), cancellationToken);
        return Ok(ApiResponse<List<AttachmentDto>>.Ok(result));
    }

    [HttpDelete("temp-attachments")]
    public async Task<ActionResult<ApiResponse<string>>> ClearTemporaryAttachments([FromQuery] TemporaryAttachmentQueryDto query, CancellationToken cancellationToken)
    {
        await _declarationService.ClearTemporaryAttachmentsAsync(query.TempAttachmentKey, User.GetUserId(), cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "清理成功"));
    }

    [HttpGet("attachments/{attachmentId:long}/download")]
    public async Task<IActionResult> DownloadAttachment(long attachmentId, CancellationToken cancellationToken)
    {
        var file = await _declarationService.DownloadAttachmentAsync(attachmentId, User.GetUserId(), cancellationToken);
        return File(file.Content, file.ContentType, file.FileName);
    }
}
