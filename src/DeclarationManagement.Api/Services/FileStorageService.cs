using Microsoft.Extensions.Options;

namespace DeclarationManagement.Api.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

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

    public async Task<byte[]> ReadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllBytesAsync(storagePath, cancellationToken);
    }
}
