using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.DTOs.PropertyAdDTOs;
using TeamProject.Domain.Entities;
using TeamProject.Domain.Enums;

namespace TeamProject.Persistence.Services;

public class PropertyAdService : IPropertyAdService
{
    private readonly IPropertyAdRepository _repository;
    private readonly IPropertyMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyAdService(
        IPropertyAdRepository repository,
        IPropertyMediaRepository mediaRepository,
        IFileStorageService fileStorage,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateAsync(PropertyAdCreateDto input, CancellationToken ct)
    {
        var propertyAd = _mapper.Map<PropertyAd>(input);
        await _repository.AddAsync(propertyAd);
        await _unitOfWork.SaveChangesAsync(ct);

        if (input.MediaFiles != null && input.MediaFiles.Any())
        {
            foreach (var file in input.MediaFiles)
            {
                var objectKey = await _fileStorage.SaveAsync(file.Stream, file.FileName, file.ContentType, 0, ct);

                var media = new PropertyMedia
                {
                    ObjectKey = objectKey,
                    Order = file.Order,
                    PropertyAdId = propertyAd.Id
                };
                await _mediaRepository.AddAsync(media);
            }
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }

    public async Task UpdateAsync(PropertyAdUpdateDto input, CancellationToken ct)
    {
        if (input.RemoveMediaIds != null && input.RemoveMediaIds.Any())
        {
            foreach (var mediaId in input.RemoveMediaIds)
            {
                var media = await _mediaRepository.GetByIdAsync(mediaId);
                if (media != null)
                {
                    await _fileStorage.DeleteFileAsync(media.ObjectKey, ct);
                    _mediaRepository.Delete(media);
                }
            }
        }
        if (input.NewMediaFiles != null && input.NewMediaFiles.Any())
        {
            foreach (var file in input.NewMediaFiles)
            {
                var objectKey = await _fileStorage.SaveAsync(file.Stream, file.FileName, file.ContentType, 0, ct);

                await _mediaRepository.AddAsync(new PropertyMedia
                {
                    ObjectKey = objectKey,
                    Order = file.Order,
                    PropertyAdId = input.Id
                });
            }
        }

        var existingAd = await _repository.GetByIdAsync(input.Id);
        if (existingAd != null)
        {
            _mapper.Map(input, existingAd);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var medias = await _mediaRepository.GetByPropertyAdIdAsync(id, ct);
        foreach (var media in medias)
        {
            await _fileStorage.DeleteFileAsync(media.ObjectKey, ct);
            _mediaRepository.Delete(media);
        }
        var ad = await _repository.GetByIdAsync(id);
        if (ad != null)
        {
            _repository.Delete(ad);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }

    public async Task<List<PropertyAdGetAllDto>> GetAllAsync()
    {
        var ads = await _repository.GetAll().ToListAsync();
        return _mapper.Map<List<PropertyAdGetAllDto>>(ads);
    }

    public async Task<PropertyAdGetByIdDto> GetByIdAsync(int id)
    {
        var ad = await _repository.GetByIdAsync(id);
        return _mapper.Map<PropertyAdGetByIdDto>(ad);
    }
}
