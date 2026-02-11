using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.DistrictDTOs;

public class DistrictCreateRequestDto
{
    public string Name { get; set; } = null!;
    public int CityId { get; set; } 
}
