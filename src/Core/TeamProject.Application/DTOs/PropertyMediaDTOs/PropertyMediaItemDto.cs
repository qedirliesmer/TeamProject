using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.PropertyMediaDTOs;

public class PropertyMediaItemDto
{
    public int Id { get; set; }
    public string ObjectKey { get; set; } = null!; 
    public int Order { get; set; }
}
