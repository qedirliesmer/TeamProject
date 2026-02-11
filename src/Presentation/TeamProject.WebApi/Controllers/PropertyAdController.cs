using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.DTOs.PropertyAdDTOs;
using TeamProject.Application.DTOs.PropertyMediaDTOs;
namespace TeamProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyAdController : ControllerBase
{
    private readonly IPropertyAdService _propertyAdService;
    private readonly IPropertyMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public PropertyAdController(
        IPropertyAdService propertyAdService,
        IPropertyMediaRepository mediaRepository,
        IFileStorageService fileStorage,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _propertyAdService = propertyAdService;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PropertyAdCreateDto dto, IFormFileCollection? media, CancellationToken ct)
    {
        var streams = new List<Stream>();
        try
        {
            if (media != null)
            {
                dto.MediaFiles = media.Select(f =>
                {
                    var stream = f.OpenReadStream();
                    streams.Add(stream);
                    return new MediaUploadInput
                    {
                        Stream = stream,
                        FileName = f.FileName,
                        ContentType = f.ContentType,
                        Order = 0 
                    };
                }).ToList();
            }

            await _propertyAdService.CreateAsync(dto, ct);
            return Ok();
        }
        finally
        {
            foreach (var stream in streams) await stream.DisposeAsync();
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] PropertyAdUpdateDto dto, IFormFileCollection? addMedia, [FromForm] int[]? removeMediaIds, CancellationToken ct)
    {
        var streams = new List<Stream>();
        try
        {
            dto.Id = id;
            dto.RemoveMediaIds = removeMediaIds?.ToList();

            if (addMedia != null)
            {
                dto.NewMediaFiles = addMedia.Select(f =>
                {
                    var stream = f.OpenReadStream();
                    streams.Add(stream);
                    return new MediaUploadInput { Stream = stream, FileName = f.FileName, ContentType = f.ContentType };
                }).ToList();
            }

            await _propertyAdService.UpdateAsync(dto, ct);
            return Ok("Updated successfully.");
        }
        finally
        {
            foreach (var stream in streams) await stream.DisposeAsync();
        }
    }

    [HttpPost("{propertyId}/media")]
    public async Task<IActionResult> UploadMedia(int propertyId, IFormFile file, CancellationToken ct)
    {
        var existingMediaCount = (await _mediaRepository.GetByPropertyAdIdAsync(propertyId, ct)).Count;
        if (existingMediaCount >= 5)
            return BadRequest("Maksimum 5 media əlavə edə bilərsiniz.");

        using var stream = file.OpenReadStream();
        var objectKey = await _fileStorage.SaveAsync(stream, file.FileName, file.ContentType, 0, ct);

        var media = new PropertyMedia
        {
            ObjectKey = objectKey,
            Order = existingMediaCount + 1,
            PropertyAdId = propertyId
        };

        await _mediaRepository.AddAsync(media);
        await _unitOfWork.SaveChangesAsync(ct);

        return Ok(_mapper.Map<PropertyMediaItemDto>(media));
    }

    [HttpGet("{propertyId}/media")]
    public async Task<IActionResult> GetMedia(int propertyId, CancellationToken ct)
    {
        var mediaList = await _mediaRepository.GetByPropertyAdIdAsync(propertyId, ct);
        return Ok(_mapper.Map<List<PropertyMediaItemDto>>(mediaList));
    }

    [HttpDelete("media/{id}")]
    public async Task<IActionResult> DeleteMedia(int id, CancellationToken ct)
    {
        var media = await _mediaRepository.GetByIdAsync(id);
        if (media == null) return NotFound();

        await _fileStorage.DeleteFileAsync(media.ObjectKey, ct);
        _mediaRepository.Delete(media);
        await _unitOfWork.SaveChangesAsync(ct);

        return NoContent();
    }
}
