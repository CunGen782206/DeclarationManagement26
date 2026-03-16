using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<List<StatisticsItemDto>>>> Query([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _statisticsService.QueryAsync(query, cancellationToken);
        return Ok(ApiResponse<List<StatisticsItemDto>>.Ok(result));
    }

    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportExcel([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var file = await _statisticsService.ExportExcelAsync(query, cancellationToken);
        return File(file.Content, file.ContentType, file.FileName);
    }

    [HttpPost("export/archive")]
    public async Task<IActionResult> ExportArchive([FromBody] StatisticsQueryDto query, CancellationToken cancellationToken)
    {
        var file = await _statisticsService.ExportArchiveAsync(query, cancellationToken);
        return File(file.Content, file.ContentType, file.FileName);
    }
}
