using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Application.DTOs.PropertyAdDTOs;
using TeamProject.Application.Options;
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
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly EmailOptions _emailOptions;

    public PropertyAdService(
        IPropertyAdRepository repository,
        IPropertyMediaRepository mediaRepository,
        IFileStorageService fileStorage,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailService emailService, 
        UserManager<User> userManager, 
        IOptions<EmailOptions> emailOptions)
    {
        _repository = repository;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _userManager = userManager;
        _emailOptions = emailOptions.Value;
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
        try
        {
            var user = await _userManager.FindByIdAsync(propertyAd.UserId);

            if (user != null)
            {
                var baseUrl = "https://localhost:7050";
                var detailLink = $"{baseUrl}/api/PropertyAd/{propertyAd.Id}";

                var subject = "Yeni Əmlak Elanı Təsdiqi";
                var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 20px;'>
                    <h2 style='color: #2d89ef;'>Təbriklər, {user.FullName}!</h2>
                    <p>Yeni bir əmlak elanınız uğurla sistemə əlavə edildi.</p>
                    <hr/>
                    <p><strong>Elan Başlığı:</strong> {propertyAd.Title}</p>
                    <p>Elanın detallarına baxmaq və yoxlamaq üçün aşağıdakı düyməyə klikləyin:</p>
                    <div style='margin-top: 20px;'>
                        <a href='{detailLink}' style='background-color: #4CAF50; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; display: inline-block;'>Elanı Görüntülə</a>
                    </div>
                    <p style='margin-top: 20px; font-size: 12px; color: #777;'>Əgər bu elanı siz yerləşdirməmisinizsə, dərhal bizimlə əlaqə saxlayın.</p>
                </div>";

                var plainText = $"Salam {user.FullName}, yeni elanınız əlavə edildi. Detallar: {detailLink}";

                await _emailService.SendEmailAsync(user.Email!, subject, htmlBody, plainText);
            }
        }
        catch (Exception ex)
        {
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
        var ads = await _repository.GetAll()
                               .Include(x => x.MediaItems)
                               .ToListAsync();

        return _mapper.Map<List<PropertyAdGetAllDto>>(ads);
    }

    public async Task<PropertyAdGetByIdDto> GetByIdAsync(int id)
    {
        var ad = await _repository.GetWithDetailsAsync(id);

        if (ad == null) return null;

        return _mapper.Map<PropertyAdGetByIdDto>(ad);
    }
}
