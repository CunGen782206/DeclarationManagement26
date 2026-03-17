using ClosedXML.Excel;
using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO.Compression;

namespace DeclarationManagement.Api.Services;

public class StatisticsService : IStatisticsService
{
    private readonly AppDbContext _dbContext;

    public StatisticsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<List<StatisticsItemDto>> QueryAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        ValidateDateRange(query.StartDate, query.EndDate);
        var data = await BuildQuery(query).ToListAsync(cancellationToken);
        return data.Select(ToDto).ToList();
    }

    public async Task<ExportFileDto> ExportExcelAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        ValidateDateRange(query.StartDate, query.EndDate);
        var data = await BuildQuery(query).ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("统计导出");

        string[] headers =
        [
            "序号", "部门", "项目名称", "项目类别", "项目等级", "奖项级别", "参与形式", "负责人", "联系方式",
            "盖章单位及时间", "审核部门", "处理意见", "原因", "认定项目等级", "认定奖项级别", "认定金额", "备注"
        ];

        for (var i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
        }

        for (var i = 0; i < data.Count; i++)
        {
            var row = i + 2;
            var item = ToDto(data[i]);

            ws.Cell(row, 1).Value = i + 1;
            ws.Cell(row, 2).Value = item.DepartmentName;
            ws.Cell(row, 3).Value = item.ProjectName;
            ws.Cell(row, 4).Value = item.ProjectCategoryName;
            ws.Cell(row, 5).Value = item.ProjectLevel.ToString();
            ws.Cell(row, 6).Value = item.AwardLevel.ToString();
            ws.Cell(row, 7).Value = item.ParticipationType.ToString();
            ws.Cell(row, 8).Value = item.ApplicantName;
            ws.Cell(row, 9).Value = item.ContactPhone;
            ws.Cell(row, 10).Value = item.SealUnitAndDate ?? string.Empty;
            ws.Cell(row, 11).Value = item.FinalReviewDepartmentName;
            ws.Cell(row, 12).Value = item.StatusName;
            ws.Cell(row, 13).Value = item.ReviewReason ?? string.Empty;
            ws.Cell(row, 14).Value = item.RecognizedProjectLevel?.ToString() ?? string.Empty;
            ws.Cell(row, 15).Value = item.RecognizedAwardLevel?.ToString() ?? string.Empty;
            ws.Cell(row, 16).Value = item.RecognizedAmount;
            ws.Cell(row, 17).Value = item.Remark ?? string.Empty;
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

    public async Task<ExportFileDto> ExportArchiveAsync(StatisticsQueryDto query, CancellationToken cancellationToken = default)
    {
        ValidateDateRange(query.StartDate, query.EndDate);
        var data = await BuildQuery(query)
            .Where(x => x.CurrentStatus == DeclarationStatus.InitialReviewApproved)
            .ToListAsync(cancellationToken);

        await using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var declaration in data)
            {
                var folder = Sanitize($"{declaration.Department?.Name}-{declaration.PrincipalName}-{declaration.ProjectName}");
                var pdfEntry = archive.CreateEntry($"{folder}/{folder}.pdf");

                await using (var entryStream = pdfEntry.Open())
                {
                    var bytes = BuildDeclarationPdf(declaration);
                    await entryStream.WriteAsync(bytes, cancellationToken);
                }

                foreach (var attachment in declaration.Attachments.Where(x => !x.IsDeleted))
                {
                    if (!File.Exists(attachment.StoragePath))
                    {
                        continue;
                    }

                    var fileEntry = archive.CreateEntry($"{folder}/附件/{attachment.OriginalFileName}");
                    await using var entryStream = fileEntry.Open();
                    var bytes = await File.ReadAllBytesAsync(attachment.StoragePath, cancellationToken);
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

    private IQueryable<Declaration> BuildQuery(StatisticsQueryDto query)
    {
        var q = _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .Include(x => x.Attachments)
            .Include(x => x.ReviewRecords)
            .AsQueryable();

        if (query.TaskId.HasValue)
        {
            q = q.Where(x => x.TaskId == query.TaskId.Value);
        }

        if (query.StartDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt >= query.StartDate.Value.Date);
        }

        if (query.EndDate.HasValue)
        {
            var endExclusive = query.EndDate.Value.Date.AddDays(1);
            q = q.Where(x => x.SubmittedAt < endExclusive);
        }

        if (query.DepartmentIds is { Count: > 0 })
        {
            q = q.Where(x => query.DepartmentIds.Contains(x.DepartmentId));
        }

        if (query.CategoryIds is { Count: > 0 })
        {
            q = q.Where(x => query.CategoryIds.Contains(x.ProjectCategoryId));
        }

        if (query.Statuses is { Count: > 0 })
        {
            q = q.Where(x => query.Statuses.Contains(x.CurrentStatus));
        }

        return q;
    }

    private static StatisticsItemDto ToDto(Declaration declaration)
    {
        var latestReview = declaration.ReviewRecords
            .OrderByDescending(x => x.ReviewedAt)
            .FirstOrDefault();

        return new StatisticsItemDto
        {
            DeclarationId = declaration.Id,
            ProjectName = declaration.ProjectName,
            ProjectCategoryName = declaration.ProjectCategory?.Name ?? string.Empty,
            DepartmentName = declaration.Department?.Name ?? string.Empty,
            ApplicantName = declaration.PrincipalName,
            ProjectLevel = declaration.ProjectLevel,
            AwardLevel = declaration.AwardLevel,
            ParticipationType = declaration.ParticipationType,
            ContactPhone = declaration.ContactPhone,
            SealUnitAndDate = declaration.SealUnitAndDate,
            FinalReviewDepartmentName = ResolveFinalReviewDepartmentName(declaration),
            StatusName = declaration.CurrentStatus.ToString(),
            ReviewReason = latestReview?.Reason,
            RecognizedProjectLevel = latestReview?.RecognizedProjectLevel,
            RecognizedAwardLevel = latestReview?.RecognizedAwardLevel,
            RecognizedAmount = latestReview?.RecognizedAmount,
            Remark = latestReview?.Remark,
            Status = declaration.CurrentStatus,
            SubmittedAt = declaration.SubmittedAt
        };
    }

    private static byte[] BuildDeclarationPdf(Declaration declaration)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Content().Column(col =>
                {
                    col.Item().Text("教学质量工程项目申报表").FontSize(18).Bold();
                    col.Item().Text($"项目名称：{declaration.ProjectName}");
                    col.Item().Text($"负责人：{declaration.PrincipalName}");
                    col.Item().Text($"所属部门：{declaration.Department?.Name}");
                    col.Item().Text($"项目类别：{declaration.ProjectCategory?.Name}");
                    col.Item().Text($"项目内容：{declaration.ProjectContent}");
                    col.Item().Text($"项目成果：{declaration.ProjectAchievement}");
                    col.Item().Text($"状态：{declaration.CurrentStatus}");
                });
            });
        }).GeneratePdf();
    }

    private static string ResolveFinalReviewDepartmentName(Declaration declaration)
    {
        return declaration.CurrentStatus switch
        {
            DeclarationStatus.PendingInitialReview or DeclarationStatus.InitialReviewApproved or DeclarationStatus.InitialReviewNotPassed or DeclarationStatus.InitialReviewRejected => "初审",
            DeclarationStatus.PendingPreReview or DeclarationStatus.PreReviewNotPassed or DeclarationStatus.PreReviewRejected => declaration.Department?.Name ?? string.Empty,
            _ => string.Empty
        };
    }

    private static string Sanitize(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        return name;
    }

    private static void ValidateDateRange(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value.Date > endDate.Value.Date)
        {
            throw new InvalidOperationException("开始日期不能晚于结束日期");
        }
    }
}
