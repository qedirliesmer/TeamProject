using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.PropertyAdDTOs;

public class PropertyAdCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public double Price { get; set; }
    public bool IsExtract { get; set; }
    public bool IsMortgage { get; set; }
    public int OfferType { get; set; }
    public int PropertyCategory { get; set; }
    public List<MediaUploadInput> MediaFiles { get; set; } = new();

}

public class MediaUploadInput
{
    public Stream Stream { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public int Order { get; set; }
}