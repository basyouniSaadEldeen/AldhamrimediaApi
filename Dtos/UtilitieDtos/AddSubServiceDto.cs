namespace AldhamrimediaApi.Dtos.UtilitieDtos
{
    public class AddSubServiceDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid utilitieId { get; set; }
    }
}
