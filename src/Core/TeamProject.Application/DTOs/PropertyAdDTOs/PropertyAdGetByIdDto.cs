using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.DTOs.PropertyAdDTOs;

public class PropertyAdGetByIdDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public double Price { get; set; }
    public bool IsExtract { get; set; }
    public bool IsMortgage { get; set; }
    public string OfferTypeName { get; set; }
    public string CategoryName { get; set; }
    public string OwnerFullName { get; set; }
    public string OwnerEmail { get; set; }
}
