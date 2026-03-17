using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 统计Controller类。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatisticsController : ControllerBase
{
    /// <summary>
    /// 统计服务字段。
    /// </summary>
    private readonly IStatisticsService _statisticsService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    /// <summary>
    /// 查询处理。
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<List<StatisticsItemDto>>>> Query([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _statisticsService.QueryAsync(query, cancellationToken); // result：结果
        return Ok(ApiResponse<List<StatisticsItemDto>>.Ok(result));
    }

    /// <summary>
    /// 导出处理。
    /// </summary>
    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportExcel([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var file = await _statisticsService.ExportExcelAsync(query, cancellationToken); // file：文件
        return File(file.Content, file.ContentType, file.FileName);
    }

    /// <summary>
    /// 导出处理。
    /// </summary>
    [HttpPost("export/archive")]
    public async Task<IActionResult> ExportArchive([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var file = await _statisticsService.ExportArchiveAsync(query, cancellationToken); // file：文件
        return File(file.Content, file.ContentType, file.FileName);
    }
}
