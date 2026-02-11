using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.CityDTOs;

public class CityCreateRequestDto
{
    public string Name { get; set; } = null!;
    public decimal Area { get; set; }
    public DateTime CreatedAt { get; set; }
}
