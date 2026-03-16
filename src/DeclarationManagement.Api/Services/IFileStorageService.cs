namespace DeclarationManagement.Api.Services;

public interface IFileStorageService
{
    Task<(string StorageFileName, string StoragePath, long Size)> SaveAsync(IFormFile file, string subFolder, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsync(string storagePath, CancellationToken cancellationToken = default);
}
