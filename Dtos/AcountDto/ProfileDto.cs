namespace AldhamrimediaApi.Dtos.AcountDto
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string phoneNumber { get; set; } = null!;
        public decimal My_balance { get; set; }



        public string ImageUrl { get; set; } = null!;

    }
}
