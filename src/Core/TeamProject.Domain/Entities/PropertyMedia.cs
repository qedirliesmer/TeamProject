using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Domain.Entities;

public class PropertyMedia:BaseEntity<int>
{
    public string ObjectKey { get; set; } = default!;
    public int Order { get; set; }
    public int PropertyAdId { get; set; }
    public PropertyAd PropertyAd { get; set; } = default!;

}
