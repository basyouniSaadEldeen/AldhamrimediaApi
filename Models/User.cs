using AldhamrimediaApi.Enums;
using Microsoft.AspNetCore.Identity;

namespace AldhamrimediaApi.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public decimal My_balance { get; set; } 

        public string Country { get; set; } = string.Empty;
        public string Country_code { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public virtual ICollection<Notifications>? Notifications { get; set; }
        public virtual ICollection<Purchases>? Purchases { get; set; }


    }
}
