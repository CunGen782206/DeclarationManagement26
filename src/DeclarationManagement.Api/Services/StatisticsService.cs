using ClosedXML.Excel;
using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO.Compression;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// StatisticsService 类。
/// </summary>
public class StatisticsService : IStatisticsService
{
    /// <summary>
    /// 数据库上下文，用于统计查询与导出。
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public StatisticsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// 查询统计列表（按筛选条件返回申报集合）。
    /// </summary>
    public async Task<List<StatisticsItemDto>> QueryAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        // q：可继续叠加条件的查询对象
        var q = BuildQuery(query);
        // data：数据库结果集
        var data = await q.ToListAsync(cancellationToken);
        return data.Select(ToDto).ToList();
    }

    /// <summary>
    /// 导出 Excel（基于当前筛选结果）。
    /// </summary>
    public async Task<ExportFileDto> ExportExcelAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        // data：待导出的申报数据
        var data = await BuildQuery(query).ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("统计导出");

        string[] headers = ["序号", "部门", "项目名称", "项目类别", "项目等级", "奖项级别", "参与形式", "负责人", "联系方式", "处理意见"];
        for (var i = 0; i < headers.Length; i++) ws.Cell(1, i + 1).Value = headers[i];

        for (var i = 0; i < data.Count; i++)
        {
            var row = i + 2;
            var d = data[i];
            ws.Cell(row, 1).Value = i + 1;
            ws.Cell(row, 2).Value = d.Department?.Name ?? string.Empty;
            ws.Cell(row, 3).Value = d.ProjectName;
            ws.Cell(row, 4).Value = d.ProjectCategory?.Name ?? string.Empty;
            ws.Cell(row, 5).Value = d.ProjectLevel.ToString();
            ws.Cell(row, 6).Value = d.AwardLevel.ToString();
            ws.Cell(row, 7).Value = d.ParticipationType.ToString();
            ws.Cell(row, 8).Value = d.PrincipalName;
            ws.Cell(row, 9).Value = d.ContactPhone;
            ws.Cell(row, 10).Value = d.CurrentStatus.ToString();
        }

        ws.Columns().AdjustToContents();

        await using var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return new ExportFileDto
        {
            FileName = $"statistics_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = ms.ToArray()
        };
    }

    /// <summary>
    /// 归档导出 ZIP：仅导出初审通过表单，包含 PDF 与附件。
    /// </summary>
    public async Task<ExportFileDto> ExportArchiveAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        // data：归档候选集（只保留初审通过）
        var data = await BuildQuery(query)
            .Where(x => x.CurrentStatus == DeclarationStatus.InitialReviewApproved)
            .Include(x => x.Attachments)
            .ToListAsync(cancellationToken);

        // zipStream：最终压缩包内存流
        await using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var d in data)
            {
                // folder：每个表单归档目录（部门-负责人-项目名称）
                var folder = Sanitize($"{d.Department?.Name}-{d.PrincipalName}-{d.ProjectName}");
                var pdfPath = $"{folder}/{folder}.pdf";
                var pdfEntry = archive.CreateEntry(pdfPath);
                await using (var entryStream = pdfEntry.Open())
                {
                    var bytes = BuildDeclarationPdf(d);
                    await entryStream.WriteAsync(bytes, cancellationToken);
                }

                foreach (var a in d.Attachments.Where(x => !x.IsDeleted))
                {
                    if (!File.Exists(a.StoragePath)) continue;
                    var fileEntry = archive.CreateEntry($"{folder}/附件/{a.OriginalFileName}");
                    await using var entryStream = fileEntry.Open();
                    var bytes = await File.ReadAllBytesAsync(a.StoragePath, cancellationToken);
                    await entryStream.WriteAsync(bytes, cancellationToken);
                }
            }
        }

        return new ExportFileDto
        {
            FileName = $"archive_{DateTime.Now:yyyyMMddHHmmss}.zip",
            ContentType = "application/zip",
            Content = zipStream.ToArray()
        };
    }

    /// <summary>
    /// 构建统计查询（统一复用筛选逻辑）。
    /// </summary>
    private IQueryable<Declaration> BuildQuery(StatisticsQueryDto query)
    {
        var q = _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .Include(x => x.Attachments)
            .AsQueryable();

        if (query.TaskId.HasValue) q = q.Where(x => x.TaskId == query.TaskId.Value);
        if (query.StartDate.HasValue) q = q.Where(x => x.SubmittedAt >= query.StartDate.Value);
        if (query.EndDate.HasValue) q = q.Where(x => x.SubmittedAt <= query.EndDate.Value);
        if (query.DepartmentIds is { Count: > 0 }) q = q.Where(x => query.DepartmentIds.Contains(x.DepartmentId));
        if (query.CategoryIds is { Count: > 0 }) q = q.Where(x => query.CategoryIds.Contains(x.ProjectCategoryId));
        if (query.Statuses is { Count: > 0 }) q = q.Where(x => query.Statuses.Contains(x.CurrentStatus));

        return q;
    }

    /// <summary>
    /// 将申报实体映射为统计列表 DTO。
    /// </summary>
    private static StatisticsItemDto ToDto(Declaration x) => new()
    {
        DeclarationId = x.Id,
        ProjectName = x.ProjectName,
        DepartmentName = x.Department?.Name ?? string.Empty,
        ApplicantName = x.PrincipalName,
        ContactPhone = x.ContactPhone,
        Status = x.CurrentStatus,
        SubmittedAt = x.SubmittedAt
    };

    /// <summary>
    /// 生成单份申报 PDF。
    /// </summary>
    private static byte[] BuildDeclarationPdf(Declaration d)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Content().Column(col =>
                {
                    col.Item().Text("教学质量工程项目申报表").FontSize(18).Bold();
                    col.Item().Text($"项目名称：{d.ProjectName}");
                    col.Item().Text($"负责人：{d.PrincipalName}");
                    col.Item().Text($"所属部门：{d.Department?.Name}");
                    col.Item().Text($"项目类别：{d.ProjectCategory?.Name}");
                    col.Item().Text($"项目内容：{d.ProjectContent}");
                    col.Item().Text($"项目成果：{d.ProjectAchievement}");
                    col.Item().Text($"状态：{d.CurrentStatus}");
                });
            });
        }).GeneratePdf();
    }

    /// <summary>
    /// 清理非法文件名字符，避免生成目录失败。
    /// </summary>
    private static string Sanitize(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        return name;
    }
}
