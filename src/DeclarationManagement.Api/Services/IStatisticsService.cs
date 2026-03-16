using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 统计服务接口。
/// </summary>
public interface IStatisticsService
{
    Task<List<StatisticsItemDto>> QueryAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
    Task<ExportFileDto> ExportExcelAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
    Task<ExportFileDto> ExportArchiveAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
}
