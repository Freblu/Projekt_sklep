using Minio;
using Minio.DataModel.Args;
using PartsCom.Application.Interfaces;

namespace PartsCom.Infrastructure.Services;

public class MinioStorageService(IMinioClient minioClient) : IFileStorageService
{
    public async Task<string> UploadFileAsync(string bucketName, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        BucketExistsArgs? beArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        bool found = await minioClient.BucketExistsAsync(beArgs, cancellationToken);

        if (!found)
        {
            MakeBucketArgs? mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);

            await minioClient.MakeBucketAsync(mbArgs, cancellationToken);
        }

        PutObjectArgs? putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType);

        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        // Return the object name or a URL if needed. 
        // For now, returning the fileName as the identifier.
        return fileName;
    }

    public async Task<Stream> GetFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();

        GetObjectArgs? args = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await minioClient.GetObjectAsync(args, cancellationToken);

        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        RemoveObjectArgs? args = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);

        await minioClient.RemoveObjectAsync(args, cancellationToken);
    }
}
