using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Enums;

namespace TeamProject.Domain.Entities;

public class PropertyAd:BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public double Price { get; set; }
    public bool IsExtract { get; set; }
    public bool IsMortgage { get; set; }
    public OfferType OfferType { get; set; }
    public PropertyCategory PropertyCategory { get; set; }
    public ICollection<PropertyMedia> MediaItems { get; set; } = [];

}
