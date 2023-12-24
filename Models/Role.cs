using Microsoft.AspNetCore.Identity;

namespace AldhamrimediaApi.Models
{
    public class Role : IdentityRole<int>
    {
        public string Description { set; get; } = string.Empty;
    }
}
