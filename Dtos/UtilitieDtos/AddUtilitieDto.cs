using AldhamrimediaApi.Enums;

namespace AldhamrimediaApi.Dtos.UtilitieDtos
{
    public class AddUtilitieDto
    {
        
        public TypeOfService Type { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsManagementService { get; set; }

        public IFormFile? LogoImage { get; set; } = null!;
        public IFormFile? PosterImage { get; set; } = null!;


    }
}
