using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Domain.Entities;

public class City:BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public decimal Area { get; set; }
    public ICollection<District> Districts { get; set; }
}
