namespace AldhamrimediaApi.Dtos.UtilitieDtos
{
    public class GetServiceDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrlLogo { get; set; } = string.Empty;
        public string ImageUrlPoster { get; set; } = string.Empty;
        public List<SubserviceDto>? subservices { get; set; }


    }
     public class SubserviceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
