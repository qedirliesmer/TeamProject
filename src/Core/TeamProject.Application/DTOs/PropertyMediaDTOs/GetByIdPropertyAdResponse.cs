using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.PropertyMediaDTOs;

public class GetByIdPropertyAdResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PropertyMediaItemDto> Media { get; set; } = new();
}
