using LighthouseSocial.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace LighthouseSocial.Infrastructure.Storage;

internal sealed class PhotoStorageService : IPhotoStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public PhotoStorageService(IOptions<MinioSettings> options)
    {
        var settings = options.Value;

        _bucketName = settings.BucketName;
        _minioClient = new MinioClient()
            .WithEndpoint(settings.Endpoint)
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .WithSSL(settings.UseSSL)
            .Build();
    }

    public async Task<Stream> GetAsync(string filePath)
    {
        using (var ms = new MemoryStream())
        {
            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(filePath)
                    .WithCallbackStream((stream) =>
                    {
                        stream.CopyTo(ms);
                    }));

            ms.Position = 0;

            return ms;
        }
    }

    public async Task<string> SaveAsync(Stream content, string fileName)
    {
        bool IsBucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));

        if (!IsBucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }

        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(content)
                .WithObjectSize(content.Length)
                .WithContentType("application/octet-stream"));

        return $"{_bucketName}/{fileName}";
    }

    public async Task DeleteAsync(string filePath)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filePath));
    }
}
