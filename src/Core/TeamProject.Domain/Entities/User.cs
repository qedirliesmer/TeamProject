using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Domain.Entities;

public class User:IdentityUser
{
    public string? FullName { get; set; }
    public ICollection<PropertyAd> PropertyAds { get; set; } = new List<PropertyAd>();
}
