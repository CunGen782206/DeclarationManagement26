namespace DeclarationManagement.Api.Services;

/// <summary>
/// IFile存储服务接口。
/// </summary>
public interface IFileStorageService
{
    Task<(string StorageFileName, string StoragePath, long Size)> SaveAsync(IFormFile file, string subFolder, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsync(string storagePath, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default);
}
