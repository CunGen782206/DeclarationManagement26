using Microsoft.Extensions.Options;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 文件存储服务类。
/// </summary>
public class FileStorageService : IFileStorageService
{
    /// <summary>
    /// 配置字段。
    /// </summary>
    private readonly FileStorageOptions _options;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// SaveAsync 方法。
    /// </summary>
    public async Task<(string StorageFileName, string StoragePath, long Size)> SaveAsync(IFormFile file, string subFolder, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_options.RootPath);
        var targetFolder = Path.Combine(_options.RootPath, subFolder); // targetFolder：target文件夹
        Directory.CreateDirectory(targetFolder);

        var storageFileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}"; // storageFileName：存储文件名称
        var fullPath = Path.Combine(targetFolder, storageFileName); // fullPath：完整路径

        await using var stream = new FileStream(fullPath, FileMode.CreateNew);
        await file.CopyToAsync(stream, cancellationToken);

        return (storageFileName, fullPath, file.Length);
    }

    /// <summary>
    /// ReadAsync 方法。
    /// </summary>
    public async Task<byte[]> ReadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllBytesAsync(storagePath, cancellationToken);
    }

    /// <summary>
    /// DeleteAsync 鏂规硶銆?
    /// </summary>
    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (File.Exists(storagePath))
        {
            File.Delete(storagePath);
        }

        return Task.CompletedTask;
    }
}
