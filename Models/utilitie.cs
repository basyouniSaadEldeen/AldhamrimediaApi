using AldhamrimediaApi.Enums;

namespace AldhamrimediaApi.Models
{
    public class utilitie
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public bool IsManagementService { get; set; }

        public string Description { get; set; }=string.Empty;
      
        public string ImageUrlLogo { get; set; } = string.Empty;
        public string ImagePublicIdLogo { get; set; } = string.Empty;
        public string ImageUrlPoster { get; set; } = string.Empty;
        public string ImagePublicIdPoster { get; set; } = string.Empty;
        public decimal Service_Cost { get; set;}

        public virtual ICollection<SubService>? subServices { get; set; }



    }
}
