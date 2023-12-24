namespace AldhamrimediaApi.Models
{
    public class SubService
    {
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        public Guid utilitieId { get; set; }
        public virtual utilitie? Utilitie { get; set; }
    }
}
