using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Domain.Entities;

public class PropertyMedia:BaseEntity<int>
{
    public  string MediaUrl { get; set; }
    public string MediaName { get; set; }
    public int Order { get; set; } = 0;
    public int PropertyAdId { get; set; }
    public PropertyAd PropertyAd{ get; set; }
}
