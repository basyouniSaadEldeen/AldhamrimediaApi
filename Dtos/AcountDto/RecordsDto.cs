namespace AldhamrimediaApi.Dtos.AcountDto
{
    public class GetRecordsDto
    {
        public Guid Id { get; set; }
        public string Type_of_service { get; set; } = null!;
        public string Service_name { get; set; } = null!;
        public string Service_ImgUrl { get; set; } = null!;
        public DateTime Service_request_date { get; set; }
        public decimal Number_of_money_paid { get; set; }
    }

    public class Buy_Service_Dto
    {
        //public int User_Id { get; set; }
        public Guid service_Id { get; set; }

        //public string Type_of_service { get; set; } = null!;
        //public string Service_name { get; set; } = null!;
        //public string Service_ImgUrl { get; set; } = null!;
        //public DateTime Service_request_date { get; set; }
        public decimal Number_of_money_paid { get; set; }
        public int Required_quantity { get; set; }
        public string Account_link { get; set; } = null!;


    }
}
