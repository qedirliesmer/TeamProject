using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Options;

namespace TeamProject.Infrastructure.Services;

public class S3MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _options;

    public S3MinioFileStorageService(IMinioClient minioClient, IOptions<MinioOptions> options)
    {
        _minioClient = minioClient;
        _options = options.Value;
    }

    public async Task<string> SaveAsync(
     Stream fileStream,
     string fileName,
     string contentType,
     int someValue = 0, 
     CancellationToken cancellationToken = default)
    {
        var objectKey = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_options.Bucket);

        if (!await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken))
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_options.Bucket);
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.Bucket)
            .WithObject(objectKey)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        return objectKey;
    }
    public async Task DeleteFileAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_options.Bucket)
            .WithObject(objectKey);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}
