using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

public interface IStatisticsService
{
    Task<List<StatisticsItemDto>> QueryAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
    Task<ExportFileDto> ExportExcelAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
    Task<ExportFileDto> ExportArchiveAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default);
}
