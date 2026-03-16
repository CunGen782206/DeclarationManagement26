using Microsoft.Extensions.Options;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// FileStorageService 类。
/// </summary>
public class FileStorageService : IFileStorageService
{
    /// <summary>
    /// _options 字段。
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
        var targetFolder = Path.Combine(_options.RootPath, subFolder);
        Directory.CreateDirectory(targetFolder);

        var storageFileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(targetFolder, storageFileName);

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
}
