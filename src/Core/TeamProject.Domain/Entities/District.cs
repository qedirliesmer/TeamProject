using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Domain.Entities;

public class District:BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public City City { get; set; } 
}
