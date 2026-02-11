using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace TeamProject.Application.Abstracts.Services;


public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string fileName, string contentType, int propertyAdId, CancellationToken ct = default);

    Task DeleteFileAsync(string objectKey, CancellationToken ct = default);
}

