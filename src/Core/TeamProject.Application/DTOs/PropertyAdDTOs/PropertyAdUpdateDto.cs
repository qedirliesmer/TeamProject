using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.PropertyAdDTOs;

public class PropertyAdUpdateDto
{
    public int Id { get; set; } 
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int>? RemoveMediaIds { get; set; }
    public List<MediaUploadInput>? NewMediaFiles { get; set; }
}
