namespace PartsCom.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string bucketName, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> GetFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
}
