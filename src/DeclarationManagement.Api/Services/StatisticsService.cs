using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using DeclarationManagement.Api.Repositories;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 统计服务实现（当前为骨架实现，后续接入 Excel/PDF/归档）。
/// </summary>
public class StatisticsService : IStatisticsService
{
    private readonly IRepository<Declaration> _declarationRepository;

    public StatisticsService(IRepository<Declaration> declarationRepository)
    {
        _declarationRepository = declarationRepository;
    }

    public async Task<List<StatisticsItemDto>> QueryAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        var declarations = await _declarationRepository.GetListAsync(cancellationToken: cancellationToken);

        return declarations.Select(x => new StatisticsItemDto
        {
            DeclarationId = x.Id,
            ProjectName = x.ProjectName,
            Status = x.CurrentStatus,
            SubmittedAt = x.SubmittedAt
        }).ToList();
    }

    public Task<ExportFileDto> ExportExcelAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        // TODO: 使用 ClosedXML 按筛选条件导出。
        return Task.FromResult(new ExportFileDto
        {
            FileName = "statistics.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = []
        });
    }

    public Task<ExportFileDto> ExportArchiveAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        // TODO: 仅归档初审通过，使用 QuestPDF + 附件打包。
        return Task.FromResult(new ExportFileDto
        {
            FileName = "archive.zip",
            ContentType = "application/zip",
            Content = []
        });
    }
}
