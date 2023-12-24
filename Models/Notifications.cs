namespace AldhamrimediaApi.Models
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public int userId { get; set; }
        public virtual User? User { get; set; }
        public string message { get; set; }=String.Empty;
    }
}
